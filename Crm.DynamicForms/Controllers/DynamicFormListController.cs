namespace Crm.DynamicForms.Controllers
{
	using System.Collections.Generic;

	using Crm.Controllers;
	using Crm.DynamicForms.Model;
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

	using Microsoft.AspNetCore.Mvc;

	public class DynamicFormListController : GenericListController<DynamicForm>
	{
		public DynamicFormListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<DynamicForm>> rssFeedProviders, IEnumerable<ICsvDefinition<DynamicForm>> csvDefinitions, IEntityConfigurationProvider<DynamicForm> entityConfigurationProvider, IRepository<DynamicForm> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Index, Group = DynamicFormsPlugin.PermissionGroup.DynamicForms)]
		public override ActionResult FilterTemplate()
		{
			return base.FilterTemplate();
		}

		[RequiredPermission(PermissionName.Index, Group = DynamicFormsPlugin.PermissionGroup.DynamicForms)]
		public override ActionResult IndexTemplate()
		{
			return base.IndexTemplate();
		}

		[RequiredPermission(PermissionName.Create, Group = DynamicFormsPlugin.PermissionGroup.DynamicForms)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}
	}
}
