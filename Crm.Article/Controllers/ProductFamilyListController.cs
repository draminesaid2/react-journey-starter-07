namespace Crm.Article.Controllers
{
	using System.Collections.Generic;

	using Crm.Article.Model;
	using Crm.Controllers;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc;

	public class ProductFamilyListController : GenericListController<ProductFamily>
	{
		protected override string GetTitle()
		{
			return "ProductFamilies";
		}
		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("ProductFamilyEmptySlate");
		}

		[RequiredPermission(PermissionName.Index, Group = ArticlePlugin.PermissionGroup.ProductFamily)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();
		[RequiredPermission(PermissionName.Index, Group = ArticlePlugin.PermissionGroup.ProductFamily)]
		public override ActionResult IndexTemplate()
		{
			return base.IndexTemplate();
		}
		[RequiredPermission(PermissionName.Create, Group = ArticlePlugin.PermissionGroup.ProductFamily)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}
		[RenderAction("ProductFamilyItemTemplateActions")]
		public virtual ActionResult ActionDetails()
		{
			return PartialView();
		}
		public ProductFamilyListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<ProductFamily>> rssFeedProviders, IEnumerable<ICsvDefinition<ProductFamily>> csvDefinitions, IEntityConfigurationProvider<ProductFamily> entityConfigurationProvider, IRepository<ProductFamily> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
	}
}
