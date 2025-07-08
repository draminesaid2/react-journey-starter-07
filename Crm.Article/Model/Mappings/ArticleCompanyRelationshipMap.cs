namespace Crm.Article.Model.Mappings
{
	using System;

	using Crm.Article.Model.Relationships;
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;

	public class ArticleCompanyRelationshipMap : EntityClassMapping<ArticleCompanyRelationship>
	{
		public ArticleCompanyRelationshipMap()
		{
			Schema("CRM");
			Table("ArticleCompanyRelationship");

			Id(x => x.Id, m =>
			{
				m.Column("ArticleCompanyRelationshipId");
				m.Generator(LMobile.Unicore.NHibernate.GuidCombGeneratorDef.Instance);
				m.UnsavedValue(Guid.Empty);
			});

			Property(x => x.RelationshipTypeKey, m => m.Column("RelationshipType"));

			Property(x => x.ParentId, m => m.Column("ArticleKey"));
			ManyToOne(x => x.Parent, m =>
			{
				m.Column("ArticleKey");
				m.Insert(false);
				m.Update(false);
				m.Lazy(LazyRelation.Proxy);
			});
			Property(x => x.ChildId, m => m.Column("CompanyKey"));
			ManyToOne(x => x.Child, m =>
			{
				m.Column("CompanyKey");
				m.Insert(false);
				m.Update(false);
				m.Lazy(LazyRelation.Proxy);
			});
			Property(x => x.Information);
		}
	}
}