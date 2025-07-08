namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;

	public class Communication : RestEntity, IRestEntityWithExtensionValues
	{
		public Guid Id { get; set; }
		public Guid ContactId { get; set; }
		public Guid? AddressId { get; set; }
		[NotReceived]
		public string LegacyId { get; set; }
		public string TypeKey { get; set; }
		public string Data { get; set; }
		public string Comment { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
	}
}