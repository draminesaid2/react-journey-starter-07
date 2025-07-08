namespace Crm.Article.Model.Mappings
{
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class ProductFamilyMap : SubclassMapping<ProductFamily>
	{
		public ProductFamilyMap()
		{
			DiscriminatorValue("ProductFamily");
			Join("ProductFamily", join =>
			{
				join.Schema("CRM");
				join.Key(x => x.Column("ContactKey"));
				join.Property(x => x.StatusKey);
				ManyToOne(x => x.ParentProductFamily,
					m =>
					{
						m.Column("ParentKey");
						m.Fetch(FetchKind.Select);
						m.Insert(false);
						m.Update(false);
						m.Lazy(LazyRelation.Proxy);
					});

				join.EntitySet(x => x.ChildProductFamilies, m =>
				{
					m.Schema("CRM");
					m.Table("Contact");
					m.Key(km => km.Column("ParentKey"));
					m.Fetch(CollectionFetchMode.Select);
					m.Lazy(CollectionLazy.Lazy);
					m.Cascade(Cascade.Persist);
					m.Inverse(true);
				}, action => action.OneToMany());
			});
		}
	}
}