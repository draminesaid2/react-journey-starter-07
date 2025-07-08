namespace Crm.Rest.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;
	using Crm.Model;
	using Crm.Model.Enums;

	[RestTypeFor(DomainType = typeof(Message))]
	public class MessageRest : IRestEntity, IRestEntityWithExtensionValues
	{
		public ICollection<Guid> AttachmentIds { get; set; }
		public List<string> Bcc { get; set; }
		public string Body { get; set; }
		[NotReceived]
		public DateTime CreateDate { get; set; }
		[NotReceived]
		public string CreateUser { get; set; }
		public string ErrorMessage { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
		public string From { get; set; }
		public Guid Id { get; set; }
		public bool IsBodyHtml { get; set; }
		[NotReceived]
		public DateTime ModifyDate { get; set; }
		[NotReceived]
		public string ModifyUser { get; set; }
		public List<string> Recipients { get; set; }
		public MessageState State { get; set; }
		public string Subject { get; set; }
	}
}
