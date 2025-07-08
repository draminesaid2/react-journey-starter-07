namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Model.Relationships;

	[RestTypeFor(DomainType = typeof(BusinessRelationship))]
	public class BusinessRelationshipRest : RestEntityWithExtensionValues
	{
		public Guid ParentId { get; set; }
		public Guid ChildId { get; set; }
		[NotReceived, ExplicitExpand] public CompanyRest Child { get; set; }
		public string Information { get; set; }
		public string RelationshipTypeKey { get; set; }
	}
}
