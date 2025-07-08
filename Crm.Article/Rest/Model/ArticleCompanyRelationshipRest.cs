namespace Crm.Article.Rest.Model
{
	using System;

	using Crm.Article.Model.Relationships;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Rest.Model;

	[RestTypeFor(DomainType = typeof(ArticleCompanyRelationship))]
	public class ArticleCompanyRelationshipRest : RestEntityWithExtensionValues
	{
		public Guid ChildId { get; set; }
		public string Information { get; set; }
		public Guid ParentId { get; set; }
		public string RelationshipTypeKey { get; set; }
		[ExplicitExpand, NotReceived] public CompanyRest Child { get; set; }
		[ExplicitExpand, NotReceived] public ArticleRest Parent { get; set; }
	}
}
