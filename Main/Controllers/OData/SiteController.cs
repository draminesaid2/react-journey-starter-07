namespace Crm.Controllers.OData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using Autofac;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Api.Extensions;
	using Crm.Library.Api.Mapping;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Services.Interfaces;
	using Crm.Rest.Model;
	using Crm.Services.Interfaces;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.OData.Formatter;
	using Microsoft.AspNetCore.OData.Query;
	using Site = Crm.Library.Model.Site.Site;

	[ControllerName("Main_Site")]
	public class SiteController : ODataController<Site, Rest.Model.Site, Guid>
	{
		private readonly IODataMapper mapper;
		private readonly ISiteService siteService;
		private readonly IEnvironment environment;
		private readonly IPluginProvider pluginProvider;
		public SiteController(ISiteService siteService, IEnvironment environment, IPluginProvider pluginProvider, IRepositoryWithTypedId<Site, Guid> modelRepository, IODataMapper mapper, IRuleValidationService ruleValidationService, IResourceManager resourceManager, ILifetimeScope lifetimeScope, IAuthorizationManager authorizationManager, IUserService userService, IEnumerable<IODataQueryFunction> queryFunctions, IEnumerable<IODataWriteFunction<Site, Rest.Model.Site, Guid>> writeFunctions, IEnumerable<IODataAlternativeKeyProvider<Site, Rest.Model.Site, Guid>> alternativeKeyProviders, ISessionHelper sessionHelper, IUsedLookupsService usedLookupsService, IEnumerable<IUsedEntityService<Site>> usedEntityServices, ISessionProvider sessionProvider, IRssFeedProvider<Site> rssFeedProvider = null, ICsvDefinition<Site> csvDefinition = null, ISyncService<Site> syncService = null)
			: base(modelRepository, mapper, ruleValidationService, resourceManager, lifetimeScope, authorizationManager, userService, queryFunctions, writeFunctions, alternativeKeyProviders, sessionHelper, usedLookupsService, usedEntityServices, sessionProvider, rssFeedProvider, csvDefinition, syncService)
		{
			this.mapper = mapper;
			this.siteService = siteService;
			this.environment = environment;
			this.pluginProvider = pluginProvider;
		}
		public const string ParameterPluginNames = "PluginNames";

		[HttpGet]
		public override IActionResult Get([FromQuery] ODataQueryOptions<Rest.Model.Site> options, [FromRoute] Guid key)
		{
			syncService = null;
			return siteService.CurrentSite.UId.Equals(key) ? GetCurrentSite() : base.Get(options, key);
		}
		[HttpGet]
		public override IActionResult Get([FromQuery] ODataQueryOptions<Rest.Model.Site> options)
		{
			syncService = null;
			return base.Get(options);
		}
		[HttpGet]
		public virtual IActionResult GetActivePluginNames()
		{
			var activePluginNames = environment.GetActivePluginNames();
			return Ok(activePluginNames);
		}
		[HttpGet]
		public virtual IActionResult GetCurrentSite()
		{
			var result = mapper.Map<Rest.Model.Site>(siteService.CurrentSite);
			return Ok(result);
		}
		[HttpGet]
		public virtual IActionResult GetMaterialThemes()
		{
			var result = MaterialCacheManifestRegistrar.Themes.Select(x => new Theme { Name = x.Key, Color = x.Value });
			return Ok(result);
		}
		[HttpGet]
		public virtual IActionResult GetPlugins(ODataQueryOptions<Rest.Model.Site> options)
		{
			var pluginDescriptors = pluginProvider.AllPluginDescriptors.Where(x => x.PluginName != "Crm.Library").ToList();
			if (options.Filter != null)
			{
				var settings = new ODataQuerySettings { EnableConstantParameterization = false, HandleNullPropagation = HandleNullPropagationOption.False };
				var queryable = pluginDescriptors.Select(x => new Rest.Model.Site { Name = x.PluginName }).AsQueryable();
				var filteredNames = ((IEnumerable<Rest.Model.Site>)options.Filter.ApplyTo(queryable, settings)).Select(x => x.Name).ToList();
				pluginDescriptors = pluginDescriptors.Where(x => filteredNames.Contains(x.PluginName)).ToList();
			}

			return Ok(mapper.Map<PluginDescriptor[]>(pluginDescriptors));
		}
		[HttpPut]
		[RequiredPermission(PermissionName.Settings, Group = PermissionGroup.WebApi)]
		public override IActionResult Put(Guid key, [FromBody] Rest.Model.Site site)
		{
			var currentSite = (Site)((ICloneable)siteService.CurrentSite).Clone();
			mapper.Map(site, currentSite);
			if (IsValid(currentSite, currentSite.UId, out var errorResponse) == false)
			{
				return errorResponse;
			}
			siteService.SaveSite(currentSite);
			var savedSite = mapper.Map<Rest.Model.Site>(currentSite);
			return StatusCode((int)HttpStatusCode.OK, savedSite);
		}
		[HttpPost]
		public virtual IActionResult SetActivePlugins(ODataActionParameters parameters)
		{
			var activePlugins = parameters.GetValue<IEnumerable<string>>(ParameterPluginNames).ToList();
			environment.SaveActivePluginNames(activePlugins);
			return Ok(activePlugins);
		}
	}
}
