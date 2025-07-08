namespace Crm.BackgroundServices.Dropbox
{
	using System;
	using System.IO;
	using System.Net.Sockets;
	using System.Threading;

	using Crm.Extensions;
	using Crm.Library.AutoFac;
	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Environment.FileSystems.AppDataFolder;
	using Crm.Library.Environment.Network;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Services.Interfaces;

	using log4net;
	using MailKit.Net.Pop3;
	using MailKit.Security;

	using Microsoft.Extensions.Caching.Distributed;
	using Microsoft.Extensions.Hosting;
	using MimeKit;
	using Quartz;

	public class DropboxAgentCache : Cache<string>, ISingletonDependency
	{
		public DropboxAgentCache(IDistributedCache cache, IRestSerializer serializer)
			: base(nameof(DropboxAgentCache), cache, serializer)
		{
		}
	}

	[DisallowConcurrentExecution]
	public class DropboxAgent : ManualSessionHandlingJobBase
	{
		private const string SuccessfulMessages = "SuccessfulMessages";
		private readonly IAppDataFolder appDataFolder;
		private readonly IAppSettingsProvider appSettingsProvider;
		protected virtual Pop3Client Pop3Client { get; set; }
		private readonly IDropboxMessageService messageService;
		private readonly DropboxAgentCache cache;

		protected virtual bool DropboxLogMessages => appSettingsProvider.GetValueOrDefault(MainPlugin.Settings.Dropbox.DropboxLogMessages, false);

		// Methods
		protected override void Run(IJobExecutionContext context)
		{
			using (Pop3Client = new Pop3Client())
			{
				var host = context.MergedJobDataMap.GetString("host");
				var port = context.MergedJobDataMap.GetInt("port");
				var username = context.MergedJobDataMap.GetString("username");
				var password = context.MergedJobDataMap.GetString("password");
				var useSsl = context.MergedJobDataMap.GetBoolean("useSsl");

				if (!TryConnectAndAuthenticate(host, port, username, password, useSsl))
				{
					return;
				}


				var mailCount = GetMailCount();
				for (var i = 0; i < mailCount; i++)
				{
					try
					{
						MimeMessage message;
						DropboxMessage dropboxMessage;
						var logMessageId = DateTime.Now.Ticks;

						// Trying to retrieve message from mail server
						if (!TryGetMessage(i, out message))
						{
							continue;
						}

						if (DropboxLogMessages)
						{
							LogMessageToDisk(message, logMessageId);
						}

						// If a dropbox message has been already successfully saved, ignore this mail and delete it from the mail server
						if (message.MessageId != null && cache.DictContains(SuccessfulMessages, message.MessageId))
						{
							DeleteMessage(i);
							continue;
						}

						// Trying to parse message and create a dropbox message
						if (!TryParseMessage(message, i, out dropboxMessage))
						{
							LogMessageToDisk(message, logMessageId);
							DeleteMessage(i);
							continue;
						}

						// Should the message be ignored?
						if (dropboxMessage.Ignore != null)
						{
							HandleIgnore(i, dropboxMessage);
							LogMessageToDisk(message, logMessageId);
							DeleteMessage(i);
							continue;
						}

						// Did the parsed message contain errors?
						if (dropboxMessage.Error != null)
						{
							HandleError(i, dropboxMessage);
							LogMessageToDisk(message, logMessageId);
							DeleteMessage(i);
							continue;
						}

						if (message.Headers == null)
						{
							LogMessageToDisk(message, logMessageId);
							DeleteMessage(i);
							throw new Exception("Message.Headers is null");
						}
						if (message.MessageId == null)
						{
							LogMessageToDisk(message, logMessageId);
							DeleteMessage(i);
							throw new Exception("MessageId is null");
						}

						BeginTransaction();
						if (messageService.Save(dropboxMessage, message))
						{
							cache.DictSet(SuccessfulMessages, message.MessageId, null);
							DeleteMessage(i);
						}
						else
						{
							Logger.Error("There was a problem when trying to save message.");
							LogMessageToDisk(message, logMessageId);
						}

						EndTransaction();
					}
					catch (Exception)
					{
						RollbackTransaction();
					}
				}
				Pop3Client.Disconnect(true);
			}
		}
		protected virtual bool TryConnectAndAuthenticate(string host, int port, string username, string password, bool useSsl)
		{
			try
			{
				if (!NetworkConnectionHelper.IsNetworkAvailable(0))
				{
					return false;
				}

				Logger.DebugFormat("Trying to connect to pop3 server {0}:{1}, SSL {2}.", host, port, useSsl);
				Pop3Client.Connect(host, port, useSsl ? SecureSocketOptions.Auto : SecureSocketOptions.None);
				Pop3Client.Authenticate(username, password);
				Logger.DebugFormat("Successfully connected and authenticated to pop3 server.");
				return true;
			}
			catch (SocketException socketException)
			{
				Logger.Warn(String.Format("IOException {0}:{1}, SocketException {2}", host, port, socketException.SocketErrorCode.ToString()), socketException);

				// Await before reconnecting again to prevent stressing the infrastructure too much -> 15 secs
				Thread.Sleep(TimeSpan.FromMilliseconds(15000));

				return false;
			}
			catch (AuthenticationException invalidLoginException)
			{
				Logger.Error(String.Format("Could not connect to {0}:{1}, Invalid login credentials", host, port), invalidLoginException);
				throw new JobExecutionException(invalidLoginException);
			}
			catch (IOException ioException)
			{
				if (ioException.InnerException is SocketException socketException)
				{
					Logger.Warn(String.Format("IOException {0}:{1}, SocketException {2}", host, port, socketException.SocketErrorCode.ToString()), ioException);
				}
				else
				{
					Logger.Warn(String.Format("IOException {0}:{1}, {2}", host, port, ioException.Message), ioException);
				}
				// Await before reconnecting again to prevent stressing the infrastructure too much -> 15 secs
				Thread.Sleep(TimeSpan.FromMilliseconds(15000));

				return false;
			}
			catch (Exception ex)
			{
				Logger.Error(String.Format("Could not connect to {0}:{1}", host, port), ex);
				return false;
			}
		}

		protected virtual int GetMailCount()
		{
			var mailCount = Pop3Client.GetMessageCount();
			if (mailCount > 0)
			{
				Logger.DebugFormat("Found {0} messages on pop3 server.", mailCount);
			}
			return mailCount;
		}

		protected virtual bool TryGetMessage(int i, out MimeMessage message)
		{
			Logger.DebugFormat("Trying to retrieve message {0} from pop3 server.", i);
			try
			{
				message = Pop3Client.GetMessage(i);
				Logger.DebugFormat("Successfully retrieved message {0} from pop3 server.", i);
				return true;
			} 
			catch (Exception ex)
			{
				message = null;
				Logger.Warn(String.Format("Could not retrieve message {0} from pop3 server.", i), ex);
				return false;
			}
		}

		protected virtual bool TryParseMessage(MimeMessage message, int i, out DropboxMessage dropboxMessage)
		{
			Logger.DebugFormat("Trying to parse message {0}.", i);
			if (messageService.TryParseMessage(message, out dropboxMessage))
			{
				Logger.DebugFormat("Successfully parsed message {0}.", i);
				return true;
			}
			Logger.WarnFormat("Unable to parse message {0}. The mail will be logged to the file system and deleted from the mail server.", i);
			return false;
		}

		protected virtual void LogMessageToDisk(MimeMessage message, long logMessageId, bool isError = false)
		{
			if (message == null || !DropboxLogMessages)
			{
				return;
			}

			var filename = isError
				? "{0}_mailmessage_failed.log".WithArgs(logMessageId)
				: "{0}_mailmessage.log".WithArgs(logMessageId);
			try
			{
				var messagePath = Path.Combine(appDataFolder.MapPath("log"), filename);
				message.WriteTo(messagePath);
				Logger.InfoFormat("Logging message to Disk, message saved to {0}", messagePath);
			}
			catch (Exception logException)
			{
				Logger.Error("Error saving message to log folder", logException);
			}
		}

		protected virtual void DeleteMessage(int i)
		{
			Logger.DebugFormat("Trying to delete message {0} from mail server.", i);
			Pop3Client.DeleteMessage(i);
			Logger.DebugFormat("Successfully deleted message {0} from mail server.", i);
		}

		protected virtual void HandleIgnore(int i, DropboxMessage dropboxMessage)
		{
			Logger.WarnFormat("Ignored mail {0}. The mail will be logged to the file system and deleted from the mail server.\n\n{1}",
				i, dropboxMessage.Ignore.LogMessage);
			messageService.NotifyUser(dropboxMessage);
		}

		protected virtual void HandleError(int i, DropboxMessage dropboxMessage)
		{
			Logger.WarnFormat(
				"Encountered an error for message {0}. The mail will be logged to the file system and deleted from the mail server.\n\n{1}",
				i, dropboxMessage.Error.LogMessage);
			messageService.NotifyUser(dropboxMessage);
		}

		// Constructor
		public DropboxAgent(IAppDataFolder appDataFolder
			, ISessionProvider sessionProvider
			, IAppSettingsProvider appSettingsProvider
			, IDropboxMessageService messageService
			, ILog logger
			, DropboxAgentCache cache
			, IHostApplicationLifetime hostApplicationLifetime)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.appDataFolder = appDataFolder;
			this.appSettingsProvider = appSettingsProvider;
			this.messageService = messageService;
			this.cache = cache;
		}
	}
}
