namespace Crm.BackgroundServices.Dropbox
{
	using Crm.Library.Extensions;
	using Crm.Services;

	using System.Linq;
	public class Error
	{
		public DropboxMessage DropboxMessage { get; set; }
		public ErrorReason Reason { get; set; }
		public string LogMessage
		{
			get { return GetLogMessage(); }
		}
		public string NotificationMessage
		{
			get { return GetNotificationMessage(); }
		}
		private object Parameter { get; set; }

		// Methods
		private string GetLogMessage()
		{
			if (DropboxMessage == null)
			{
				return "DropboMessage is null in method GetLogMessage.";
			}

			switch (Reason)
			{
				case ErrorReason.NoTextPlainPart:
					return "E-mail without text/plain part received";
				case ErrorReason.NoTextFound:
					return "E-mail without text/plain or text/html part received";
				case ErrorReason.InvalidDropboxAddress:
					return DropboxMessage.DropboxMailAddress != null ? "E-mail with invalid dropbox token {0} received.".WithArgs(DropboxMessage.DropboxMailAddress.User) : "Dropbox address is null.";
				case ErrorReason.InvalidEntityId:
					return "E-mail with invalid {0} Id received.".WithArgs(DropboxMessage.EntityType);
				case ErrorReason.EntityIdDoesNotExist:
					return "E-Mail with non-existing {0} Id {1} received.".WithArgs(DropboxMessage.EntityType, DropboxMessage.EntityId);
				case ErrorReason.NoUserCorrespondingToDropboxToken:
					return DropboxMessage.DropboxMailAddress != null ? "No user corresponding to dropbox address {0} found in the system.".WithArgs(DropboxMessage.DropboxMailAddress.Address) : "Dropbox address is null.";
				case ErrorReason.NoContactMailAddressFound:
					return "No contact mail address found.";
				case ErrorReason.NoContactCorrespondingToContactMailAddress:
					return "No contact with e-mail address {0} found.".WithArgs(DropboxMessage.ContactMailAddresses.FirstOrDefault());
				case ErrorReason.NoContactCorrespondingToContactId:
					return "No contact with ContactId {0} found in the system.".WithArgs((int)Parameter);
				case ErrorReason.ForwardedWithoutMailAddressInBody:
					return "Forwarded e-mail without contact mail address in the body received.";
				case ErrorReason.ForwardedWithoutValidDropboxAddress:
					return "Forwarded e-mail without valid dropbox domain address in To header received.";
				case ErrorReason.MailWithoutBccOrXEnvelopeOrReceivedHeader:
					return "E-mail received that does not contain a BCC or " + DropboxMessageService.RecipientHeaders.Join("/") + " header or no Received header containing a dropboxDomain address.";
				case ErrorReason.InvalidUserMailAddress:
					return "From is not a valid user mail address.";
				default:
					return "No log message defined for this error";
			}
		}

		private string GetNotificationMessage()
		{
			switch (Reason)
			{
				case ErrorReason.NoTextPlainPart:
					return "The dropbox agent only accepty e-mails containing a text/plain part.";
				case ErrorReason.NoTextFound:
					return "The dropbox agent only accepty e-mails containing a text/plain or text/html part.";
				case ErrorReason.InvalidDropboxAddress:
					return "You sent an e-mail to the dropbox address {0} but no such dropbox address exists in the system. Please check your current dropbox address on the settings page."
						.WithArgs(DropboxMessage.DropboxMailAddress.Address, DropboxMessage.DropboxMailAddress.Address);
				case ErrorReason.InvalidEntityId:
					return "{0} is an invalid {1} Id.".WithArgs(DropboxMessage.EntityId, DropboxMessage.EntityType);
				case ErrorReason.EntityIdDoesNotExist:
					return "No {0} with Id {1} found in the system.".WithArgs(DropboxMessage.EntityType, DropboxMessage.EntityId);
				case ErrorReason.NoUserCorrespondingToDropboxToken:
					return GetLogMessage();
				case ErrorReason.NoContactMailAddressFound:
					return GetLogMessage();
				case ErrorReason.NoContactCorrespondingToContactMailAddress:
					return GetLogMessage();
				case ErrorReason.NoContactCorrespondingToContactId:
					return "The dropbox was not able to assign your e-mail to a contact.";
				case ErrorReason.ForwardedWithoutMailAddressInBody:
					return
						"When forwarding an e-mail to the dropbox agent, the contact mail address must be provided in the body of the e-mail. The first valid e-mail address found in the body is assumed to be the contact mail address.";
				case ErrorReason.ForwardedWithoutValidDropboxAddress:
					return "When forwarding an e-mail to the dropbox the To header must contain the dropbox address.";
				case ErrorReason.InvalidUserMailAddress:
					return "Your mail address could not be assigned to a user. Dropbox messages are only accepted from your e-mail you entered in the settings page.";
				default:
					return null;
			}
		}

		// Constructor
		public Error(ErrorReason reason, DropboxMessage dropboxMessage)
		{
			DropboxMessage = dropboxMessage;
			Reason = reason;
		}

		public Error(ErrorReason reason, DropboxMessage dropboxMessage, object parameter)
			: this(reason, dropboxMessage)
		{
			Parameter = parameter;
		}
	}
}