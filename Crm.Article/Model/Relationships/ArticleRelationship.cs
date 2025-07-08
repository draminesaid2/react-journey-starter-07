namespace Crm.Article.Model.Relationships
{
	using Crm.Article.Model.Lookups;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Rest;

	public class ArticleRelationship : LookupRelationship<Article, Article, ArticleRelationshipType>, ISoftDelete, IRestEntity
	{
		public virtual decimal? QuantityValue { get; set; }
		public virtual string QuantityUnitKey { get; set; }
		public virtual QuantityUnit QuantityUnit { get { return QuantityUnitKey != null ? LookupManager.Get<QuantityUnit>(QuantityUnitKey) : null; }}
	}
}
