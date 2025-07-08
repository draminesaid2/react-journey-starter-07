namespace Crm.Controllers;

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

using Microsoft.AspNetCore.Mvc;

public class StationListController : GenericListController<Station>
{
	[RequiredPermission(PermissionName.Index, Group = nameof(Station))]
	public override ActionResult IndexTemplate() => base.IndexTemplate();

	[RequiredPermission(PermissionName.Index, Group = nameof(Station))]
	public override ActionResult FilterTemplate() => base.FilterTemplate();

	[RequiredPermission(PermissionName.Create, Group = nameof(Station))]
	public override ActionResult MaterialPrimaryAction() => PartialView();
	public StationListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Station>> rssFeedProviders, IEnumerable<ICsvDefinition<Station>> csvDefinitions, IEntityConfigurationProvider<Station> entityConfigurationProvider, IRepository<Station> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
		: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
	{
	}
}
