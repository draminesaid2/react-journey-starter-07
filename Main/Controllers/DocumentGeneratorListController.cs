namespace Crm.Controllers;

using System.Collections.Generic;

using Crm.Library.BaseModel.Interfaces;
using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Library.EntityConfiguration;
using Crm.Library.EntityConfiguration.Interfaces;
using Crm.Library.Globalization.Resource;
using Crm.Library.Helper;
using Crm.Library.Model;
using Crm.Library.Model.Authorization;
using Crm.Library.Modularization;
using Crm.Library.Modularization.Interfaces;
using Crm.Library.Rest;
using Crm.Library.Services.Interfaces;
using Crm.Rest.Model;
using Crm.ViewModels;

using Microsoft.AspNetCore.Mvc;

public class DocumentGeneratorListController : GenericListController<IEntity>
{
	public DocumentGeneratorListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<IEntity>> rssFeedProviders, IEnumerable<ICsvDefinition<IEntity>> csvDefinitions, IEntityConfigurationProvider<IEntity> entityConfigurationProvider, IRepository<IEntity> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
		: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
	{
	}

	[RequiredPermission(nameof(DocumentGeneratorEntry), Group = PermissionGroup.WebApi)]
	public override ActionResult FilterTemplate()
	{
		var model = GetGenericListTemplateViewModel();
		return PartialView("Material/GenericListRightNav", model);
	}
	protected override GenericListViewModel GetGenericListTemplateViewModel()
	{
		var model = base.GetGenericListTemplateViewModel();
		model.Title = "DocumentGeneration";
		model.TypeNameOverride = "DocumentGenerator";
		return model;
	}
	[RequiredPermission(nameof(DocumentGeneratorEntry), Group = PermissionGroup.WebApi)]
	public override ActionResult IndexTemplate()
	{
		return base.IndexTemplate();
	}
	[RequiredPermission(nameof(DocumentGeneratorEntry), Group = PermissionGroup.WebApi)]
	public override ActionResult MaterialPrimaryAction()
	{
		return new EmptyResult();
	}
	[RenderAction("DocumentGeneratorListFilterTemplate")]
	public virtual ActionResult TypeFilter()
	{
		return PartialView();
	}
}
