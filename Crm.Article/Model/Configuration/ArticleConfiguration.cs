namespace Crm.Article.Model.Configuration
{
	using Crm.Library.EntityConfiguration;

	public class ArticleConfiguration : EntityConfiguration<Article>
	{
		public override void Initialize()
		{
			//Filterable properties
			Property(x => x.Description,
				f =>
				{
					f.Filterable();
					f.Sortable();
				});
			Property(x => x.ArticleType, f => f.Filterable());
			Property(
				x => x.ItemNo,
				f =>
				{
					f.Filterable();
					f.Sortable();
				});
			Property(x => x.ManufacturerItemNo, f => f.Filterable());
			Property(x => x.IsEnabled, f => f.Filterable());
			Property(x => x.ArticleGroup01, f => f.Filterable());
			Property(x => x.ArticleGroup02, f => f.Filterable());
			Property(x => x.ArticleGroup03, f => f.Filterable());
			Property(x => x.ArticleGroup04, f => f.Filterable());
			Property(x => x.ArticleGroup05, f => f.Filterable());
			Property(x => x.ProductFamilyKey, m => m.Filterable(f => f.Definition(new AutoCompleterFilterDefinition<ProductFamily>("ProductFamilyAutocomplete", new { Plugin = "Crm.Article" }, "CrmArticle_ProductFamily", x => x.Name, x => x.Id, x => x.LegacyId, x => x.Name) { Caption = "ProductFamily" })));
			Property(x => x.IsSparePart, f => f.Filterable());
			Property(x => x.DangerousGoodsFlag, f => f.Filterable());
			Property(x => x.IsBatch, f => f.Filterable());
			Property(x => x.IsSerial, f => f.Filterable());
		}

		public ArticleConfiguration(IEntityConfigurationHolder<Article> entityConfigurationHolder)
			: base(entityConfigurationHolder)
		{
		}
	}
}
