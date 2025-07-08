namespace Crm.Article.Rest.Model
{
	using Crm.Article.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Rest.Model;

	[RestTypeFor(DomainType = typeof(ProductFamily))]
	public class ProductFamilyRest : ContactRest
	{
		public virtual string StatusKey { get; set; }
		[NotReceived, ExplicitExpand] public ProductFamilyRest ParentProductFamily { get; set; }
		[NotReceived, ExplicitExpand] public ProductFamilyRest[] ChildProductFamilies { get; set; }
		[ExplicitExpand, NotReceived] public UserRest ResponsibleUserUser { get; set; }

	}
}
