namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;
	using Crm.Model;

	[RestTypeFor(DomainType = typeof(LinkResource))]
	public class LinkResourceRest : RestEntity, IRestEntityWithExtensionValues
	{
		// Properties
		public Guid Id { get; set; }
		public virtual Guid ParentId { get; set; }
		public virtual string Url { get; set; }
		public virtual string Description { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
	}
}