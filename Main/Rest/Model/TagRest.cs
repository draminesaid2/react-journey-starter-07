namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;
	using Crm.Model;

	[RestTypeFor(DomainType = typeof(Tag))]
	public class TagRest : RestEntity, IRestEntityWithExtensionValues
	{
		public Guid Id { get; set; }
		public Guid ContactKey { get; set; }
		public string Name { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
	}
}