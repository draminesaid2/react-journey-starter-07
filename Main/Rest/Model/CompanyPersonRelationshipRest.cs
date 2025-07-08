namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Model.Relationships;

	[RestTypeFor(DomainType = typeof(CompanyPersonRelationship))]
	public class CompanyPersonRelationshipRest : RestEntityWithExtensionValues
	{
		public Guid ParentId { get; set; }
		public Guid ChildId { get; set; }
		public string Information { get; set; }
		public string RelationshipTypeKey { get; set; }
		[ExplicitExpand, NotReceived] public CompanyRest Parent { get; set; }
		[ExplicitExpand, NotReceived] public PersonRest Child { get; set; }
	}
}
