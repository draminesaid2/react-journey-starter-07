namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net.Mail;
	using System.Text;
	using System.Text.RegularExpressions;

	using Crm.BackgroundServices.Dropbox;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation;
	using Crm.Model;
	using Crm.Model.Extensions;
	using Crm.Services.Interfaces;

	using log4net;

	using Model.Notes;
	using Library.Modularization.Interfaces;
	using MimeKit;
	using MimeKit.Text;
	using Crm.Extensions;

	public class DropboxMessageService : IDropboxMessageService
	{
		private readonly IEnumerable<IDropboxMessageIgnoreRule> ignoreRules;
		private readonly IUserService userService;
		private readonly IRepositoryWithTypedId<Contact, Guid> contactRepository;
		private readonly IRepositoryWithTypedId<Model.Message, Guid> messageRepository;
		private readonly ILog logger;
		public static string[] RecipientHeaders = { "X-Envelope-To", "X-Delivered-To" };
		private readonly IPluginProvider pluginProvider;
		private readonly IRepositoryWithTypedId<Note, Guid> noteRepository;
		private readonly IRepositoryWithTypedId<FileResource, Guid> fileResourceRepository;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly Func<FileResource> fileResourceFactory;
		private readonly Func<Model.Message> messageFactory;
		private readonly Func<EmailNote> emailNoteFactory;
		private readonly IResourceManager resourceManager;

		// Methods
		public virtual bool TryParseMessage(MimeMessage message, out DropboxMessage dropboxMessage, string dropboxMail = null)
		{
			try
			{
				dropboxMessage = InitDropboxMessage(message, dropboxMail);

				FindEntityTypeAndId(dropboxMessage);
				CheckCustomIgnoreRules(message, dropboxMessage);
				CheckBusinessIgnoreRules(message, dropboxMessage);
				GetBodyFromMessage(message, dropboxMessage);
				FindDropboxToken(dropboxMessage);
				if (dropboxMessage.EntityType.IsNullOrEmpty() || dropboxMessage.EntityId.EqualsDefault())
				{
					FindContactMailAddress(message, dropboxMessage);
					ValidateContactMailAddress(dropboxMessage);
				}
				ValidateDropboxToken(dropboxMessage);

				return true;
			}
			catch (Exception ex)
			{
				logger.Error("The message parser encountered an error when trying to parse a message.\n\n", ex);
				dropboxMessage = null;
				return false;
			}
		}
		public virtual bool Save(DropboxMessage dropboxMessage, MimeMessage message)
		{
			logger.DebugFormat("Trying to save a dropbox message.");
			try
			{
				if (dropboxMessage == null)
				{
					throw new NullReferenceException("DropboxMessage is null!");
				}
				logger.DebugFormat("dropboxMessage.HeaderTo: {0}", dropboxMessage.HeaderTo);
				logger.DebugFormat("dropboxMessage.HeaderBcc: {0}", dropboxMessage.HeaderBcc);
				logger.DebugFormat("dropboxMessage.HeaderFrom: {0}", dropboxMessage.HeaderFrom);
				if (dropboxMessage.UserMailAddress == null)
				{
					throw new NullReferenceException("DropboxMessage.UserMailAddress is null!");
				}
				// The existence of contact and user were checked by the parser. But there is still a very small chance that the contact or user was deleted in the meantime.
				var user = userService.GetUsers().GetByEmail(dropboxMessage.UserMailAddress.Address) ?? userService.GetUsers().GetByDropboxToken(dropboxMessage.DropboxToken);
				if (user == null)
				{
					throw new NullReferenceException("User is null!");
				}
				if(dropboxMessage.EntityId.HasValue)
				{
					logger.DebugFormat("dropboxMessage.EntityId: {0}", dropboxMessage.EntityId);
					logger.DebugFormat("dropboxMessage.EntityType: {0}", dropboxMessage.EntityType);
					GenerateNote(dropboxMessage, null, user.Id, message);
				}
				else
				{
					foreach (var contactMailAddress in dropboxMessage.ContactMailAddresses)
					{
						var contact = contactRepository.GetAll().FirstOrDefault(x => x.Communications.OfType<Email>().Any(m => m.Data == contactMailAddress.Address));
						GenerateNote(dropboxMessage, contact, user.Id, message);
					}
				}
				logger.InfoFormat("Successfully saved a DropboxMessage.");
				return true;
			}
			catch (Exception ex)
			{
				logger.Error("Could not save a DropboxMessage.", ex);
				return false;
			}
		}
		public virtual void NotifyUser(DropboxMessage dropboxMessage)
		{
			if (dropboxMessage == null || dropboxMessage.UserMailAddress == null
					|| (dropboxMessage.Ignore == null && dropboxMessage.Error == null))
			{
				return;
			}

			var user = userService.GetUsers().GetByEmail(dropboxMessage.UserMailAddress.Address);
			if (user == null)
			{
				return;
			}

			var message = "You {0} an e-mail to the dropbox, but no note could be created for the following reason: "
				.WithArgs(dropboxMessage.IsForwarded ? "forwarded" : "sent");
			if (dropboxMessage.Ignore != null)
			{
				message += dropboxMessage.Ignore.NotificationMessage;
			}
			else if (dropboxMessage.Error != null)
			{
				message += dropboxMessage.Error.NotificationMessage;
			}

			message += "\n\n\n";
			message += "----------\n";
			message += "\n";
			message += "From: {0}\n".WithArgs(dropboxMessage.HeaderFrom);
			message += "To: {0}\n".WithArgs(dropboxMessage.HeaderTo);
			message += "BCC: {0}\n".WithArgs(dropboxMessage.HeaderBcc).If(dropboxMessage.HeaderBcc.IsNotNullOrEmpty());
			message += "Date: {0}\n\n".WithArgs(dropboxMessage.HeaderDateSent);
			message += "Subject: {0}\n\n".WithArgs(dropboxMessage.HeaderSubject);
			message += dropboxMessage.Body;

			var notificationMessage = messageFactory();
			notificationMessage.Recipients.Add(dropboxMessage.UserMailAddress.Address);
			notificationMessage.Subject = dropboxMessage.Subject;
			notificationMessage.Body = message;

			try
			{
				messageRepository.SaveOrUpdate(notificationMessage);
			}
			catch (Exception ex)
			{
				logger.ErrorFormat("User {0} could not be notified about failed handled error in dropbox agent.\n\n{0}", ex);
			}
		}

		protected virtual DropboxMessage InitDropboxMessage(MimeMessage message, string dropboxMail)
		{
			var forwardPrefixes = appSettingsProvider.GetValue(MainPlugin.Settings.Dropbox.DropboxForwardPrefixes);
			var isForwarded = message.Subject.StartsWithAny(forwardPrefixes, true);
			var dropboxMessage = new DropboxMessage
			{
				Subject = message.Subject.RemovePrefixes(forwardPrefixes),
				PlainMessage = message.HtmlBody,
				UserMailAddress = new MailAddress(message.From.GetAddresses().First()),
				IsForwarded = isForwarded,
				DropboxMailAddress = dropboxMail.IsNotNullOrEmpty() ? new MailAddress(dropboxMail) : FindDropboxAddress(message),
				Attachments = ConvertMessageAttachments(message),
				HeaderDateSent = message.Date.DateTime.ToShortDateString(),
				HeaderFrom = message.From != null ? message.From.GetAddresses().First() : null,
				HeaderTo = message.To.GetAddresses().Join(", "),
				HeaderBcc = message.Bcc.Count > 0 ? message.Bcc.GetAddresses().Join(", ") : dropboxMail,
				HeaderSubject = message.Subject
			};

			if (userService.GetUsers().GetByEmail(dropboxMessage.UserMailAddress?.Address) == null)
			{
				dropboxMessage.Error = new Error(ErrorReason.InvalidUserMailAddress, dropboxMessage);
			}

			if (dropboxMessage.IsForwarded && dropboxMessage.DropboxMailAddress == null)
			{
				dropboxMessage.Error = new Error(ErrorReason.ForwardedWithoutValidDropboxAddress, dropboxMessage);
			}

			return dropboxMessage;
		}

		protected virtual List<Attachment> ConvertMessageAttachments(MimeMessage message)
		{
			var attachments = new List<Attachment>();
			foreach (var attachment in message.Attachments)
			{
				if (attachment is MimePart mimePart)
				{
					var mimePartStream = new MemoryStream();
					mimePart.Content.DecodeTo(mimePartStream);
					mimePartStream.Position = 0;
					attachments.Add(new Attachment(mimePartStream, mimePart.FileName, mimePart.ContentType.MimeType));
				}
			}

			return attachments;
		}
		protected virtual void CheckCustomIgnoreRules(MimeMessage message, DropboxMessage dropboxMessage)
		{
			var ignoreRule = ignoreRules.FirstOrDefault(x => x.IsSatisfiedBy(message));
			if (ignoreRule != null)
			{
				dropboxMessage.Ignore = new Ignore(ignoreRule.Header, dropboxMessage);
			}
		}
		protected virtual void CheckBusinessIgnoreRules(MimeMessage message, DropboxMessage dropboxMessage)
		{
			if (dropboxMessage.Ignore != null)
			{
				return;
			}
			var dropboxDomain = appSettingsProvider.GetValue(MainPlugin.Settings.Dropbox.DropboxDomain);
			if (!dropboxMessage.HeaderTo.Split(",").Any(x => x.EndsWith(dropboxDomain)) &&
				(dropboxMessage.HeaderBcc.IsNotNullOrEmpty() && !dropboxMessage.HeaderBcc.Split(",").Any(x => x.EndsWith(dropboxDomain))) &&
				!message.Headers.Any(x => x.Value.Contains(dropboxDomain)) &&
				userService.GetUsers().GetByEmail(message.From.GetAddresses().First()) == null)
			{
				dropboxMessage.Ignore = new Ignore(IgnoreReason.NotSentToDropboxMailAddress, dropboxMessage);
			}
			
			if (dropboxMessage.IsForwarded && (dropboxMessage.EntityType.IsNullOrEmpty() || dropboxMessage.EntityId.EqualsDefault()) && !message.To.GetAddresses().Any(x => x.Trim().ToLower().EndsWith(dropboxDomain.ToLower())))
			{
				logger.Warn("A forwarded e-mail to the dropbox agent was ignored because a To address in '{0}' did not end with '{1}'.".WithArgs(message.To.GetAddresses().ToString(), dropboxDomain.ToLower()));
				dropboxMessage.Ignore = new Ignore(IgnoreReason.ForwardedMailDoesNotContainValidDropboxAddressInToHeader, dropboxMessage);
			}

			if (dropboxMessage.IsForwarded && (dropboxMessage.EntityType.IsNullOrEmpty() || dropboxMessage.EntityId.EqualsDefault()) && (message.Bcc.Count > 0 && !message.Bcc.GetAddresses().Any(x => x.Trim().ToLower().EndsWith(dropboxDomain.ToLower()))))
			{
				logger.Warn("A forwarded e-mail to the dropbox agent was ignored because a BCC address in '{0}' did not end with '{1}'.".WithArgs(message.To.ToString(), dropboxDomain.ToLower()));
				dropboxMessage.Ignore = new Ignore(IgnoreReason.ForwardedMailDoesNotContainValidDropboxAddressInBccHeader, dropboxMessage);
			}

			if (!dropboxMessage.IsForwarded && (message.To.GetAddresses().Any(x => x.ToLower().EndsWith(dropboxDomain.ToLower())) || message.Cc.GetAddresses().Any(x => x.ToLower().EndsWith(dropboxDomain.ToLower()))))
			{
				dropboxMessage.Ignore = new Ignore(IgnoreReason.MailContainsDropboxAddressInToOrCcHeader, dropboxMessage);
			}

			if (!dropboxMessage.IsForwarded &&
				dropboxMessage.HeaderTo.Split(",").Length == 0 && 
				RecipientHeaders.All(x => message.Headers[x] == null) &&
				message.To.GetAddresses().Any(x => x.Contains(dropboxDomain)) &&
				userService.GetUsers().GetByEmail(message.From.GetAddresses().First()) == null)
			{
				dropboxMessage.Error = new Error(ErrorReason.MailWithoutBccOrXEnvelopeOrReceivedHeader, dropboxMessage);
			}
		}
		protected virtual MailAddress FindDropboxAddress(MimeMessage message)
		{
			var dropboxDomain = appSettingsProvider.GetValue(MainPlugin.Settings.Dropbox.DropboxDomain);
			// Try to find matching address from To addresses
			var matchingTo = message.To.ToMailAddresses().FirstOrDefault(x => x.Host.ToLower().EndsWith(dropboxDomain.ToLower()));
			if (matchingTo != null)
			{
				return matchingTo;
			}

			// Try to find matching address from BCC addresses
			var matchingBcc = message.Bcc.ToMailAddresses().FirstOrDefault(x => x.Host.ToLower().EndsWith(dropboxDomain.ToLower()));
			if (matchingBcc != null)
			{
				return matchingBcc;
			}

			// if none found try parsing from Received headers or add recipient header message.Headers.UnknownHeaders
			var receivedHeaders = new List<string>(message.Headers.Select(x => x.Value));
			if (message.Headers != null
					&& message.Headers.Any(x => RecipientHeaders.Select(h => h.ToLower()).Contains(x.Id.ToString().ToLower())))
			{
				receivedHeaders.Add(message.Headers[message.Headers.First(x => RecipientHeaders.Select(h => h.ToLower()).Contains(x.Id.ToString().ToLower())).Id.ToString()]);
			}

			var regexDropboxAddress = new Regex(@"(?<MailAddress>[a-zA-Z0-9\-_]+@{0}+)".WithArgs(dropboxDomain));
			foreach (string receivedHeader in receivedHeaders)
			{
				if (!regexDropboxAddress.IsMatch(receivedHeader))
				{
					continue;
				}

				var matches = regexDropboxAddress.Matches(receivedHeader);
				for (var i = 0; i < matches.Count; i++)
				{
					var address = matches[i].Groups[0].Value;
					var tokens = address.Split('@');
					var userToken = tokens.First().Split('_').Last();
					if (userService.GetUsers().GetByDropboxToken(userToken) != null)
					{
						return new MailAddress(address);
					}
				}
			}

			// if none found but user valid try parsing from original sender
			var user = message.From != null ? userService.GetUsers().GetByEmail(message.From.GetAddresses().First()) : null;
			if (user != null)
			{
				return new MailAddress(user.GetDropboxAddress(dropboxDomain));
			}

			// Log error message.
			var sb = new StringBuilder();
			sb.AppendLine("DropboxMailAddress could not be determined.");
			sb.AppendLine();
			sb.AppendFormatLine("DropboxDomain: {0}", dropboxDomain);
			sb.AppendFormatLine("Subject: {0}", message.Subject);
			sb.AppendFormatLine("Headers:");
			foreach (var header in message.Headers)
			{
				sb.AppendFormatLine("{0}: {1}", header.Id.ToString(), header.Value);
			}

			logger.Warn(sb.ToString());

			return null;
		}

		protected virtual void GetBodyFromMessage(MimeMessage message, DropboxMessage dropboxMessage)
		{
			var textParts = message.BodyParts.Where(x => x is TextPart).Select(x => x as TextPart);
			if (textParts.Any(x => x.IsPlain))
			{
				dropboxMessage.Body = textParts.First(x => x.IsPlain).Text;
			}
			else if (textParts.Any(x => x.IsHtml))
			{
				dropboxMessage.Body = textParts.First(x => x.IsHtml).ConvertHtmlToPlainText();
			}
			else
			{
				dropboxMessage.Error = new Error(ErrorReason.NoTextFound, dropboxMessage);
			}
		}
		protected virtual void FindContactMailAddress(MimeMessage message, DropboxMessage dropboxMessage)
		{
			if (dropboxMessage.Ignore != null || dropboxMessage.Error != null)
			{
				return;
			}

			// Don't search for contact mail address if e-mail is forwarded to an entity dropbox mail address (e.g. project dropbox address), because the note is attached to the entity, not the contact
			if (dropboxMessage.EntityId.HasValue)
			{
				return;
			}

			if (dropboxMessage.IsForwarded)
			{
				var regexMailAddress = new Regex("(?<MailAddress>{0})".WithArgs(Formats.ValidEmailFormat));
				var contactMailAddress = dropboxMessage.Body.FindMatchingSubstring(regexMailAddress);
				if (contactMailAddress != null)
				{
					dropboxMessage.ContactMailAddresses.Add(new MailAddress(contactMailAddress));
				}
			}
			else
			{
				dropboxMessage.ContactMailAddresses = message.To.Mailboxes.Select(x => new MailAddress(x.Address)).ToList();
			}

			if (dropboxMessage.IsForwarded && !dropboxMessage.ContactMailAddresses.Any())
			{
				dropboxMessage.Error = new Error(ErrorReason.ForwardedWithoutMailAddressInBody, dropboxMessage);
			}
		}
		protected virtual void FindEntityTypeAndId(DropboxMessage dropboxMessage)
		{
			if (dropboxMessage.Ignore != null || dropboxMessage.Error != null)
			{
				return;
			}

			if (dropboxMessage.DropboxMailAddress == null)
			{
				return;
			}

			var userParts = dropboxMessage.DropboxMailAddress.User.Split('_');

			switch (userParts.Length)
			{
				case 1:
					return;
				case 3:
					var entityType = userParts[0];
					Guid entityId;
					if (!Guid.TryParse(userParts[1], out entityId))
					{
						dropboxMessage.Error = new Error(ErrorReason.InvalidEntityId, dropboxMessage);
						return;
					}

					var entity = contactRepository.Get(entityId);
					if (entity == null)
					{
						dropboxMessage.Error = new Error(ErrorReason.EntityIdDoesNotExist, dropboxMessage);
						return;
					}

					dropboxMessage.EntityType = entityType;
					dropboxMessage.EntityId = entityId;
					return;
				default:
					dropboxMessage.Error = new Error(ErrorReason.InvalidDropboxAddress, dropboxMessage);
					return;
			}
		}
		protected virtual void FindDropboxToken(DropboxMessage dropboxMessage)
		{
			if (dropboxMessage.Ignore != null || dropboxMessage.Error != null)
			{
				return;
			}

			if (dropboxMessage.DropboxMailAddress == null)
			{
				return;
			}

			var dropboxToken = dropboxMessage.DropboxMailAddress.User.Split('_').Last();
			if (dropboxToken.IsHexNumber())
			{
				dropboxMessage.DropboxToken = dropboxToken;
			}
			else
			{
				dropboxMessage.Error = new Error(ErrorReason.InvalidDropboxAddress, dropboxMessage);
			}
		}
		protected virtual void ValidateDropboxToken(DropboxMessage dropboxMessage)
		{
			if (dropboxMessage.Ignore != null || dropboxMessage.Error != null)
			{
				return;
			}

			if (userService.GetUsers().GetByDropboxToken(dropboxMessage.DropboxToken) == null)
			{
				dropboxMessage.Error = new Error(ErrorReason.NoUserCorrespondingToDropboxToken, dropboxMessage);
			}
		}
		protected virtual void ValidateContactMailAddress(DropboxMessage dropboxMessage)
		{
			if (dropboxMessage.Ignore != null || dropboxMessage.Error != null)
			{
				return;
			}

			if (!dropboxMessage.ContactMailAddresses.Any())
			{
				dropboxMessage.Error = new Error(ErrorReason.NoContactMailAddressFound, dropboxMessage);
				return;
			}
			var validatedContactMailAddresses = new List<MailAddress>();
			foreach (var contactMailAddress in dropboxMessage.ContactMailAddresses)
			{
				var contact = contactRepository.GetAll().FirstOrDefault(x => x.Communications.Any(c => c.Data == contactMailAddress.Address));
				if (contact != null && !validatedContactMailAddresses.Contains(contactMailAddress))
					validatedContactMailAddresses.Add(contactMailAddress);
			}
			dropboxMessage.ContactMailAddresses = validatedContactMailAddresses;
			if (!dropboxMessage.ContactMailAddresses.Any())
			{
				dropboxMessage.Error = new Error(ErrorReason.NoContactCorrespondingToContactMailAddress, dropboxMessage);
			}
		}

		protected virtual void GenerateNote(DropboxMessage dropboxMessage, Contact contact, string username, MimeMessage message)
		{
			var contactType = contact?.GetType();
			var pluginName = contactType != null ? pluginProvider.FindPluginDescriptor(contactType)?.PluginName : null;

			// TODO: Refactor it out as it seems an odd check here
			if (contactType == null)
			{
				switch (dropboxMessage.EntityType)
				{
					case "Project":
						pluginName = "Crm.Project";
						break;
				}

			}
			logger.DebugFormat("contactType: {0}", contactType);
			logger.DebugFormat("pluginName: {0}", pluginName);

			logger.DebugFormat("DropboxMessage.EntityId: {0}", dropboxMessage?.EntityId);
			logger.DebugFormat("DropboxMessage.EntityType: {0}", dropboxMessage?.EntityType);
			logger.DebugFormat("ContactType: {0}", contactType);
			logger.DebugFormat("PluginName: {0}", pluginName);
			
			// Only if original EML message was passed down we'll try to save it
			if (message != null)
			{
				try
				{
					var messageStream = new MemoryStream();
					message.WriteTo(messageStream);
					messageStream.Position = 0;
					var messageAsEml = new Attachment(messageStream, "[" + resourceManager.GetTranslation("Email") + "]: " + message.Subject + ".eml");
					dropboxMessage.Attachments.Add(messageAsEml);
				}
				catch(Exception ex)
				{
					logger.Error("Couldn't attach message as .eml\n", ex);
				}
			}

			var note = emailNoteFactory();
			note.Subject = dropboxMessage.Subject;
			note.Text = dropboxMessage.Body;
			note.ContactId = dropboxMessage.EntityId ?? contact.Id;
			note.IsActive = true;
			note.CreateUser = username;
			note.ModifyUser = username;
			note.Plugin = pluginName;
			noteRepository.SaveOrUpdate(note);

			var maxSize = appSettingsProvider.GetValue(MainPlugin.Settings.Dropbox.MinFileSizeInBytes);
			var filesWithoutName = 0;
			foreach (Attachment attachment in dropboxMessage.Attachments)
			{
				try
				{
					if (attachment.Name.IsNullOrEmpty())
					{
						filesWithoutName++;
					}
					var content = attachment.ContentStream.ReadAllBytes();
					var contentTypeAttachment = attachment.ContentType.MediaType.Split('/')[0];
					if (contentTypeAttachment == "image" && content.Length < maxSize)
					{
						logger.InfoFormat("Skipped file attachment {0} because of file size {1} bytes", attachment.Name, content.Length);
							continue;
					}
					var fileResource = fileResourceFactory();
					fileResource.Filename = attachment.Name ?? "Untitled" + filesWithoutName + MimeTypeMapping.GetFileExtensionByMimeType(attachment.ContentType.MediaType);
					fileResource.ContentType = attachment.ContentType.MediaType;
					fileResource.Length = content.Length;
					fileResource.Content = content;
					fileResource.CreateUser = username;
					fileResource.ModifyUser = username;
					fileResource.ParentId = note.Id;
					fileResourceRepository.SaveOrUpdate(fileResource);
				}
				catch (Exception ex)
				{
					logger.Error("Could not attach file.\n", ex);
				}
			}
		}

		public DropboxMessageService(
			IUserService userService,
			IRepositoryWithTypedId<Contact, Guid> contactRepository,
			IAppSettingsProvider appSettingsProvider,
			IRepositoryWithTypedId<Model.Message, Guid> messageRepository,
			IEnumerable<IDropboxMessageIgnoreRule> ignoreRules,
			IPluginProvider pluginProvider,
			IRepositoryWithTypedId<Note, Guid> noteRepository,
			IRepositoryWithTypedId<FileResource, Guid> fileResourceRepository,
			ILog logger,
			Func<FileResource> fileResourceFactory,
			Func<Model.Message> messageFactory,
			Func<EmailNote> emailNoteFactory,
			IResourceManager resourceManager)
		{
			this.userService = userService;
			this.contactRepository = contactRepository;
			this.messageRepository = messageRepository;
			this.ignoreRules = ignoreRules ?? new List<IDropboxMessageIgnoreRule>();
			this.pluginProvider = pluginProvider;
			this.noteRepository = noteRepository;
			this.fileResourceRepository = fileResourceRepository;
			this.logger = logger;
			this.fileResourceFactory = fileResourceFactory;
			this.messageFactory = messageFactory;
			this.emailNoteFactory = emailNoteFactory;
			this.resourceManager = resourceManager;
			this.appSettingsProvider = appSettingsProvider;
		}
	}
}
