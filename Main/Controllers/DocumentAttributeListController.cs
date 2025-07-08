namespace Crm.Controllers
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;

	using Microsoft.AspNetCore.Mvc;

	public class DocumentAttributeListController : GenericListController<DocumentAttribute>
	{
		public DocumentAttributeListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<DocumentAttribute>> rssFeedProviders, IEnumerable<ICsvDefinition<DocumentAttribute>> csvDefinitions, IEntityConfigurationProvider<DocumentAttribute> entityConfigurationProvider, IRepository<DocumentAttribute> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}

		[RequiredPermission(PermissionName.Index, Group = MainPlugin.PermissionGroup.DocumentAttribute)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();
		protected override string GetEmptySlate()
		{
			if (!repository.GetAll().Any())
			{
				return resourceManager.GetTranslation("M_NoDocsFound");
			}

			return resourceManager.GetTranslation("SearchCriteriaYieldedNoResults");
		}
		protected override string GetTitle()
		{
			return "DocumentAttributes";
		}
		[RequiredPermission(PermissionName.Index, Group = MainPlugin.PermissionGroup.DocumentAttribute)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();
	}
}
