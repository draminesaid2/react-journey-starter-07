namespace Crm.BackgroundServices.Dropbox
{
	using System;
	using System.Collections.Generic;
	using System.Net.Mail;

	public class DropboxMessage
	{
		public String PlainMessage { get; set; }
		public MailAddress UserMailAddress { get; set; }
		public MailAddress DropboxMailAddress { get; set; }
		public string DropboxToken { get; set; }
		public List<MailAddress> ContactMailAddresses { get; set; }
		public string EntityType { get; set; } // Entity to which the note should be attached, e.g. Project
		public Guid? EntityId { get; set; } // Id of the entity to which the note should be attached
		public string Subject { get; set; }
		public String Body { get; set; }
		public bool IsForwarded { get; set; }
		public Ignore Ignore { get; set; }
		public Error Error { get; set; }
		public List<Attachment> Attachments { get; set; }

		public string HeaderDateSent { get; set; }
		public string HeaderFrom { get; set; }
		public string HeaderTo { get; set; }
		public string HeaderBcc { get; set; }
		public string HeaderSubject { get; set; }

		public DropboxMessage()
		{
			ContactMailAddresses = new List<MailAddress>();
			Attachments = new List<Attachment>();
		}
	}
}