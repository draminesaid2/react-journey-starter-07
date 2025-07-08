namespace Crm.Article.Controllers
{
	using System.Collections.Generic;
	using System.Linq;
	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Controllers;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Model.Lookups;

	using Microsoft.AspNetCore.Mvc;

	public class ArticleListController : GenericListController<Article>
	{
		protected override string GetTitle()
		{
			return "Article";
		}

		[RequiredPermission(PermissionName.Index, Group = ArticlePlugin.PermissionGroup.Article)]
		public override ActionResult FilterTemplate()
		{
			return base.FilterTemplate();
		}
		[RequiredPermission(PermissionName.Index, Group = ArticlePlugin.PermissionGroup.Article)]
		public override ActionResult IndexTemplate()
		{
			return base.IndexTemplate();
		}

		[RenderAction("MaterialArticleListThumbnail")]
		[RequiredPermission(PermissionName.Index, Group = ArticlePlugin.PermissionGroup.Article)]
		public virtual ActionResult MaterialArticleListThumbnail()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Create, Group = ArticlePlugin.PermissionGroup.Article)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}

		public ArticleListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Article>> rssFeedProviders, IEnumerable<ICsvDefinition<Article>> csvDefinitions, IEntityConfigurationProvider<Article> entityConfigurationProvider, IRepository<Article> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}

		public class ArticleCsvDefinition : CsvDefinition<Article>
		{
			private readonly ILookupManager lookupManager;
			public ArticleCsvDefinition(IResourceManager resourceManager, ILookupManager lookupManager)
				: base(resourceManager)
			{
			 	this.lookupManager = lookupManager;
			}
			public override string GetCsv(IEnumerable<Article> items)
			{
				var articleTypes = lookupManager.List<ArticleType>();
				var currencies = lookupManager.List<Currency>();
				var qtyUnits = lookupManager.List<QuantityUnit>();

				Property("Id", x => x.Id);
				Property("ItemNo", x => x.ItemNo);
				Property("ArticleType", x => x.ArticleTypeKey.IsNotNullOrEmpty() ? articleTypes.FirstOrDefault(c => c.Key == x.ArticleTypeKey)?.Value : string.Empty);
				Property("Price", x => x.Price);
				Property("PurchasePrice", x => x.PurchasePrice);
				Property("Currency", x => x.CurrencyKey.IsNotNullOrEmpty() ? currencies.FirstOrDefault(c => c.Key == x.CurrencyKey)?.Value : string.Empty);
				Property("QuantityUnit", x => x.QuantityUnitKey.IsNotNullOrEmpty() ? qtyUnits.FirstOrDefault(c => c.Key == x.QuantityUnitKey)?.Value : string.Empty);
				Property("Description", x => x.Description);

				//Internal Ids
				Property("ArticleTypeKey", x => x.ArticleTypeKey);
				Property("CurrencyKey", x => x.CurrencyKey);
				Property("QuantityUnitKey", x => x.QuantityUnitKey);

				return base.GetCsv(items);
			}
		}
	}
}
