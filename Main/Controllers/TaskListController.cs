namespace Crm.Controllers
{
	using System.Collections.Generic;

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

	public class TaskListController : GenericListController<Task>
	{
		public TaskListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Task>> rssFeedProviders, IEnumerable<ICsvDefinition<Task>> csvDefinitions, IEntityConfigurationProvider<Task> entityConfigurationProvider, IRepository<Task> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Index, Group = MainPlugin.PermissionGroup.Task)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();
		[RequiredPermission(MainPlugin.PermissionName.Ics, Group = MainPlugin.PermissionGroup.Task)]
		public override ActionResult GetIcsLink() => base.GetIcsLink();
		protected override string GetTitle()
		{
			return "Tasks";
		}
		[RequiredPermission(PermissionName.Index, Group = MainPlugin.PermissionGroup.Task)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();

		[RequiredPermission(PermissionName.Create, Group = MainPlugin.PermissionGroup.Task)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}
		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("TaskEmptySlate");
		}
	}
}
