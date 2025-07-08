namespace Crm.Article
{

	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Offline;
	using Crm.Library.Offline.Interfaces;

	public class MaterialCacheManifestRegistrar : CacheManifestRegistrar<MaterialCacheManifest>
	{
		public MaterialCacheManifestRegistrar(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
		}
		protected override void Initialize()
		{
			CacheJs("articleMaterialJs");
			CacheJs("articleMaterialTs");
			Cache("ArticleList", "FilterTemplate");
			Cache("ArticleList", "IndexTemplate");
			Cache("Article", "CreateTemplate");
			Cache("Article", "DetailsTemplate");
			Cache("ArticleCompanyRelationship", "EditTemplate");
			Cache("ProductFamilyList", "FilterTemplate");
			Cache("ProductFamilyList", "IndexTemplate");
			Cache("ProductFamily", "CreateTemplate");
			Cache("ProductFamily", "DetailsTemplate");
		}
	}
}
