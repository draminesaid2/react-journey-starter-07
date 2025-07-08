namespace Crm.BackgroundServices
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net.Mail;
	using System.Text;
	using System.Threading;

	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Model.Enums;
	using Crm.Services.Interfaces;

	using log4net;

	using Microsoft.Extensions.Hosting;

	using Quartz;

	[DisallowConcurrentExecution]
	public class EmailAgent : ManualSessionHandlingJobBase
	{
		// Members
		private readonly IUserService userService;
		private readonly IRepositoryWithTypedId<Message, Guid> messageRepository;
		private readonly IRepositoryWithTypedId<FileResource, Guid> fileResourceRepository;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly ISmtpClientProvider smtpClientProvider;

		public const string JobGroup = "Core";
		public const string JobName = "EmailAgent";

		// Methods
		protected override void Run(IJobExecutionContext context)
		{
			SendMessages();
		}

		protected virtual void SendMessages()
		{
			// Some SMTP Servers have a limitation in sending out mails in short rates, so smaller batches
			var batchSize = 20;
			var throttlePeriodMs = 100;

			var messages = messageRepository.GetAll().Where(x => x.State == MessageState.Pending);
			var batch = messages.Take(batchSize);

			using var smtpClient = smtpClientProvider.CreateSmtpClient();
			while (batch.Any())
			{
				foreach (var message in batch)
				{
					if (receivedShutdownSignal)
					{
						break;
					}

					SendMessage(smtpClient, message);
					// Await before sending more messages to prevent SMTP Server from thinking we're spam
					Thread.Sleep(TimeSpan.FromMilliseconds(throttlePeriodMs));
				}

				batch = messages.Take(batchSize);
			}
		}
		protected virtual void SendMessage(SmtpClient smtpClient, Message message)
		{
			if (!TryFindRecipients(message, out var recipients))
			{
				return;
			}

			try
			{
				BeginTransaction();
				using var mailMessage = smtpClientProvider.CreateMailMessage();
				mailMessage.BodyEncoding = Encoding.UTF8;
				mailMessage.Body = message.Body;
				mailMessage.IsBodyHtml = message.IsBodyHtml;
				mailMessage.Subject = message.Subject;

				if (TryFindSender(message, out var messageSender))
				{
					mailMessage.From = new MailAddress(messageSender);
				}

				mailMessage.To.AddRange(recipients);

				if (message.Bcc.Count > 0)
				{
					mailMessage.Bcc.AddRange(message.Bcc.Where(x => x.IsValidEmailAddress()).Select(x => new MailAddress(x)));
				}

				var attachments = fileResourceRepository.GetAll()
					.Where(x => message.AttachmentIds.Contains(x.Id))
					.ToList();
				mailMessage.Attachments.AddRange(attachments.Select(x => x.Content.CreateAttachment(x.ContentType, x.Filename)));

				smtpClient.Send(mailMessage);

				message.State = MessageState.Dispatched;
				messageRepository.SaveOrUpdate(message);
			}
			catch (Exception ex)
			{
				Logger.Error("Could not send message.", ex);
				message.ErrorMessage = ex.ToString();
				message.State = MessageState.Failed;
				messageRepository.SaveOrUpdate(message);
			}
			finally
			{
				EndTransaction();
			}
		}

		protected virtual bool TryFindRecipients(Message message, out ICollection<MailAddress> recipients)
		{
			recipients = new List<MailAddress>();

			if (message.Recipients.Count == 0)
			{
				return false;
			}

			var messageRecipients = message.Recipients;
			if (messageRecipients.Count > 0)
			{
				foreach (var recipient in messageRecipients)
				{
					if (recipient.IsValidEmailAddress())
					{
						recipients.Add(new MailAddress(recipient));
					}
					else
					{
						var user = userService.GetUser(recipient);
						if (user != null)
						{
							recipients.Add(new MailAddress(user.Email));
						}
					}
				}

				if (recipients.Count > 0)
				{
					return true;
				}
			}

			return false;
		}

		protected virtual bool TryFindSender(Message message, out string messageSender)
		{
			messageSender = null;
			if (appSettingsProvider.GetValue(MainPlugin.Settings.Email.SenderImpersonation) == false)
			{
				return false;
			}
			if (string.IsNullOrWhiteSpace(message.From) == false)
			{
				try
				{
					var mailAddress = new MailAddress(message.From);
					messageSender = message.From;
					return true;
				}
				catch (FormatException)
				{
				}
			}
			return false;
		}

		public static async void Trigger(IScheduler scheduler)
		{
			if (!scheduler.IsStarted)
			{
				await scheduler.Start();
			}
			var alreadyTriggered = scheduler.GetTriggersOfJob(new JobKey(JobName, JobGroup)).Result.OfType<ISimpleTrigger>().Any(x => scheduler.GetTriggerState(x.Key).Result != TriggerState.Complete && x.GetNextFireTimeUtc() != null);
			if (alreadyTriggered)
			{
				return;
			}
			var trigger = TriggerBuilder
				.Create()
				.ForJob(JobName, JobGroup)
				.StartAt(DateTime.UtcNow)
				.Build();
			await scheduler.ScheduleJob(trigger);
		}

		// Constructor
		public EmailAgent(IUserService userService, IRepositoryWithTypedId<Message, Guid> messageRepository, ISessionProvider sessionProvider, ILog logger, IAppSettingsProvider appSettingsProvider, IHostApplicationLifetime hostApplicationLifetime, ISmtpClientProvider smtpClientProvider, IRepositoryWithTypedId<FileResource, Guid> fileResourceRepository)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.userService = userService;
			this.messageRepository = messageRepository;
			this.appSettingsProvider = appSettingsProvider;
			this.smtpClientProvider = smtpClientProvider;
			this.fileResourceRepository = fileResourceRepository;
		}
	}
}
