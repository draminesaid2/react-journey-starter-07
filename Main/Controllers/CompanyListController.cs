namespace Crm.Controllers
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Model.Lookups;

	using Microsoft.AspNetCore.Mvc;

	using NHibernate.Linq;

	public class CompanyListController : GenericListController<Company>
	{
		public CompanyListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Company>> rssFeedProviders, IEnumerable<ICsvDefinition<Company>> csvDefinitions, IEntityConfigurationProvider<Company> entityConfigurationProvider, IRepository<Company> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Read, Group = MainPlugin.PermissionGroup.Company)]
		[RenderAction("CompanyItemTemplateActions", Priority = 100)]
		public virtual ActionResult ActionDetails()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Index, Group = MainPlugin.PermissionGroup.Company)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();

		protected override string GetTitle()
		{
			return "Companies";
		}
		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("CompanyEmptySlate");
		}

		[RequiredPermission(PermissionName.Index, Group = MainPlugin.PermissionGroup.Company)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();

		[RequiredPermission(PermissionName.Create, Group = MainPlugin.PermissionGroup.Company)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}

		public class CompanyCsvDefinition : CsvDefinition<Company>
		{
			private readonly ILookupManager lookupManager;
			public override IQueryable<Company> Eager(IQueryable<Company> query)
			{
				return query
					.Fetch(x => x.AreaSalesManagerObject)
					.FetchMany(x => x.Communications)
					.Fetch(x => x.ResponsibleUserObject)
					.Fetch(x => x.StandardAddress);
			}

			public CompanyCsvDefinition(IResourceManager resourceManager, ILookupManager lookupManager)
				: base(resourceManager)
			{
				this.lookupManager = lookupManager;
			}
			public override string GetCsv(IEnumerable<Company> items) {
				var companyTypes = lookupManager.List<CompanyType>();
				var sourceTypes = lookupManager.List<SourceType>();
				var noOfEmployees = lookupManager.List<NumberOfEmployees>();
				var turnovers = lookupManager.List<Turnover>();
				var countries = lookupManager.List<Country>();
				var regions = lookupManager.List<Region>();
				var languages = lookupManager.List<Language>();

				Property("Id", x => x.Id);
				Property("CompanyNo", x => x.CompanyNo ?? string.Empty);
				Property("Name", x => x.Name);
				Property("CompanyType", x =>  x.CompanyTypeKey.IsNotNullOrEmpty() ? companyTypes.FirstOrDefault(c => c.Key == x.CompanyTypeKey)?.Value : string.Empty);
				Property("T_ParentId", x => x.ParentId);
				Property("ParentCompany", x => x.ParentName);
				Property("BackgroundInfo", x => x.BackgroundInfo);
				Property("SourceType", x => x.SourceTypeKey.IsNotNullOrEmpty() ? sourceTypes.FirstOrDefault(c => c.Key == x.SourceTypeKey)?.Value : string.Empty);
				Property("NumberOfEmployees", x => x.NumberOfEmployeesKey.IsNotNullOrEmpty() ? noOfEmployees.FirstOrDefault(c => c.Key == x.NumberOfEmployeesKey)?.Value : string.Empty);
				Property("Turnover", x => x.TurnoverKey.IsNotNullOrEmpty() ? turnovers.FirstOrDefault(c => c.Key == x.TurnoverKey)?.Value : string.Empty);
				Property("Name1", x => x.StandardAddress.Name1);
				Property("Name2", x => x.StandardAddress.Name2);
				Property("Name3", x => x.StandardAddress.Name3);
				Property("City", x => x.StandardAddress.City);
				Property("Country", x => x.StandardAddress.CountryKey.IsNotNullOrEmpty() ? countries.FirstOrDefault(c => c.Key == x.StandardAddress.CountryKey)?.Value : string.Empty);
				Property("ZipCode", x => x.StandardAddress.ZipCode);
				Property("Street", x => x.StandardAddress.Street);
				Property("Region", x => x.StandardAddress.RegionKey.IsNotNullOrEmpty() ? regions.FirstOrDefault(c => c.Key == x.StandardAddress.RegionKey)?.Value : string.Empty);
				Property("POBox", x => x.StandardAddress.POBox);
				Property("ZipCodePOBox", x => x.StandardAddress.ZipCodePOBox);
				Property("Phone", x => x.PrimaryPhoneData);
				Property("Fax", x => x.PrimaryFaxData);
				Property("Email", x => x.PrimaryEmailData);
				Property("Website", x => x.Websites.FirstOrDefault());
				Property("Language", x => x.LanguageKey.IsNotNullOrEmpty() ? languages.FirstOrDefault(c => c.Key == x.LanguageKey)?.Value : string.Empty);
				Property("LegacyId", x => x.LegacyId);
				Property("ResponsibleUser", x => x.ResponsibleUserObject != null ? x.ResponsibleUserObject.DisplayName : x.ResponsibleUser);
				Property("AreaSalesManager", x => x.AreaSalesManagerObject != null ? x.AreaSalesManagerObject.DisplayName : x.AreaSalesManager);
				Property("CreateDate", x => x.CreateDate.ToShortDateString());
				Property("ModifyDate", x => x.ModifyDate.ToShortDateString());
				Property("Tags", x => x.Tags?.ToString(", "));

				//Internal Ids
				Property("CompanyTypeKey", x =>  x.CompanyTypeKey);
				Property("SourceTypeKey", x => x.SourceTypeKey);
				Property("NumberOfEmployeesKey", x => x.NumberOfEmployeesKey);
				Property("TurnoverKey", x => x.TurnoverKey);
				Property("CountryKey", x => x.StandardAddress != null ? x.StandardAddress.CountryKey : string.Empty);
				Property("RegionKey", x => x.StandardAddress != null ? x.StandardAddress.RegionKey : string.Empty);
				Property("LanguageKey", x => x.LanguageKey);

				return base.GetCsv(items);
			}
		}
	}
}
