namespace Crm.Article.Model.Relationships
{
	using Crm.Article.Model.Lookups;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Rest;
	using Crm.Model;

	public class ArticleCompanyRelationship : LookupRelationship<Article, Company, ArticleCompanyRelationshipType>, ISoftDelete, IRestEntity
	{
	}
}