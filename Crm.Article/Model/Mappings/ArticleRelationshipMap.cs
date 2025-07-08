namespace Crm.Article.Model.Mappings
{
	using System;

	using Crm.Article.Model.Relationships;
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class ArticleRelationshipMap : EntityClassMapping<ArticleRelationship>
	{
		public ArticleRelationshipMap()
		{
			Schema("CRM");
			Table("ArticleRelationship");

			Id(x => x.Id, m =>
			{
				m.Column("ArticleRelationshipId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.RelationshipTypeKey, m => m.Column("RelationshipType"));

			Property(x => x.ParentId, m => m.Column("ParentArticleKey"));
			ManyToOne(x => x.Parent, m =>
			{
				m.Column("ParentArticleKey");
				m.Insert(false);
				m.Update(false);
				m.Lazy(LazyRelation.Proxy);
			});
			Property(x => x.ChildId, m => m.Column("ChildArticleKey"));
			ManyToOne(x => x.Child, m =>
			{
				m.Column("ChildArticleKey");
				m.Insert(false);
				m.Update(false);
				m.Lazy(LazyRelation.Proxy);
			});
			Property(x => x.QuantityValue);
			Property(x => x.QuantityUnitKey, m => m.Column("QuantityUnit"));
			Property(x => x.Information);
		}
	}
}