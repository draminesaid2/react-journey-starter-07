namespace Crm.Article.Model.Mappings
{
	using Crm.Library.BaseModel.Mappings;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class ArticleMap : SubclassMapping<Article>
	{
		public ArticleMap()
		{
			DiscriminatorValue("Article");

			Join("Article", join =>
				{
					join.Schema("CRM");
					join.Key(a =>
						{
							a.Column("ArticleId");
							a.NotNullable(true);
						});
					join.Property(x => x.ItemNo);
					join.Property(x => x.ManufacturerItemNo);
					join.Property(x => x.Description);
					join.Property(x => x.ArticleTypeKey, m => m.Column("ArticleType"));
					join.Property(x => x.Price, m => m.Column("SalesPrice"));
					join.Property(x => x.QuantityUnitKey, m => m.Column("QuantityUnit"));
					join.Property(x => x.CurrencyKey);
					join.Property(x => x.IsSerial);
					join.Property(x => x.SerialRequired);
					join.Property(x => x.Weight);
					join.Property(x => x.WeightNet);
					join.Property(x => x.Length);
					join.Property(x => x.Width);
					join.Property(x => x.Height);
					join.Property(x => x.MatchCode);
					join.Property(x => x.ArticleGroup01Key, m => m.Column("ArticleGroup01"));
					join.Property(x => x.ArticleGroup02Key, m => m.Column("ArticleGroup02"));
					join.Property(x => x.ArticleGroup03Key, m => m.Column("ArticleGroup03"));
					join.Property(x => x.ArticleGroup04Key, m => m.Column("ArticleGroup04"));
					join.Property(x => x.ArticleGroup05Key, m => m.Column("ArticleGroup05"));
					join.Property(x => x.IsBatch);
					join.Property(x => x.Remark);
					join.Property(x => x.DangerousGoodsFlag);
					join.Property(x => x.MinimumStock);
					join.Property(x => x.BarCode);
					join.Property(x => x.IsSparePart);
					join.Property(x => x.IsWarehouseManaged);
					join.Property(x => x.LeadTimeInDays);
					join.Property(x => x.GuaranteeInMonths);
					join.Property(x => x.WarrantyInMonths);
					join.Property(x => x.IsEnabled);
					join.Property(x => x.ValidFrom);
					join.Property(x => x.ValidTo);
					join.Property(x => x.VATLevelKey);
					join.Property(x => x.QuantityStep);
					join.Property(x => x.PurchasePrice);
					join.Property(x => x.ProductFamilyKey);

					join.EntitySet(x => x.ArticleParentRelationships, m =>
					{
						m.Key(k =>
						{
							k.Column("ParentArticleKey");
							k.NotNullable(true);
						});
						m.Inverse(true);
						m.Lazy(CollectionLazy.Lazy);
						m.Cascade(Cascade.DeleteOrphans);
						m.BatchSize(100);
					}, action => action.OneToMany());
					join.EntitySet(x => x.ArticleChildRelationships, m =>
					{
						m.Key(k =>
						{
							k.Column("ChildArticleKey");
							k.NotNullable(true);
						});
						m.Inverse(true);
						m.Lazy(CollectionLazy.Lazy);
						m.Cascade(Cascade.None);
						m.BatchSize(100);
					}, action => action.OneToMany());
					join.ManyToOne(x => x.ProductFamily, m =>
					{
						m.Column("ProductFamilyKey");
						m.Fetch(FetchKind.Select);
						m.Insert(false);
						m.Update(false);
					});
				});
		}
	}
}
