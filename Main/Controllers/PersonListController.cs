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

	public class PersonListController : GenericListController<Person>
	{
		public PersonListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Person>> rssFeedProviders, IEnumerable<ICsvDefinition<Person>> csvDefinitions, IEntityConfigurationProvider<Person> entityConfigurationProvider, IRepository<Person> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Read, Group = MainPlugin.PermissionGroup.Person)]
		[RenderAction("PersonItemTemplateActions", Priority = 100)]
		public virtual ActionResult ActionDetails()
		{
			return PartialView();
		}
		[RequiredPermission(PermissionName.Delete, Group = MainPlugin.PermissionGroup.Person)]
		[RenderAction("PersonItemTemplateActions", Priority = 50)]
		public virtual ActionResult ActionToggleRetired()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Index, Group = MainPlugin.PermissionGroup.Person)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();

		protected override string GetTitle()
		{
			return "PeopleTitle";
		}
		protected override string GetEmptySlate()
		{
			return resourceManager.GetTranslation("PersonEmptySlate");
		}
		[RequiredPermission(PermissionName.Index, Group = MainPlugin.PermissionGroup.Person)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();

		[RequiredPermission(PermissionName.Create, Group = MainPlugin.PermissionGroup.Person)]
		public override ActionResult MaterialPrimaryAction()
		{
			return PartialView();
		}

		public class PersonCsvDefinition : CsvDefinition<Person>
		{
			private readonly IAppSettingsProvider appSettingsProvider;
			private readonly ILookupManager lookupManager;
			public override IQueryable<Person> Eager(IQueryable<Person> query)
			{
				return query
					.Fetch(x => x.Address)
					.Fetch(x => x.Parent)
					.Fetch(x => x.ResponsibleUserObject)
					.FetchMany(x => x.Communications);
			}

			public PersonCsvDefinition(IResourceManager resourceManager, IAppSettingsProvider appSettingsProvider, ILookupManager lookupManager)
				: base(resourceManager)
			{
				this.appSettingsProvider = appSettingsProvider;
				this.lookupManager = lookupManager;
			}
			public override string GetCsv(IEnumerable<Person> items) {
				var countries = lookupManager.List<Country>();
				var regions = lookupManager.List<Region>();
				var salutations = lookupManager.List<Salutation>();
				var languages = lookupManager.List<Language>();
				var titles = lookupManager.List<Title>();
				var businessTitles = appSettingsProvider.GetValue(MainPlugin.Settings.Person.BusinessTitleIsLookup) ? lookupManager.List<BusinessTitle>() : null;
				var departments = appSettingsProvider.GetValue(MainPlugin.Settings.Person.DepartmentIsLookup) ? lookupManager.List<DepartmentType>() : null;

				Property("Id", x => x.Id);
				Property("PersonNo", x => x.PersonNo ?? string.Empty);
				Property("Salutation", x => x.SalutationKey.IsNotNullOrEmpty() ? salutations.FirstOrDefault(c => c.Key == x.SalutationKey)?.Value : string.Empty);
				Property("Title", x => x.TitleKey.IsNotNullOrEmpty() ? titles.FirstOrDefault(c => c.Key == x.TitleKey)?.Value : string.Empty);
				Property("FirstName", x => x.Firstname);
				Property("LastName", x => x.Surname);
				Property("T_ParentId", x => x.ParentId);
				Property("Company", x => x.ParentName);
				Property("BackgroundInfo", x => x.BackgroundInfo);
				Property("Company_Name1", x => x.Address.Name1);
				Property("Company_Name2",x => x.Address.Name2);
				Property("Company_Name3", x => x.Address.Name3);
				Property("City", x => x.Address.City);
				Property("Country", x => x.Address.CountryKey.IsNotNullOrEmpty() ?  countries.FirstOrDefault(c => c.Key == x.Address.CountryKey)?.Value : string.Empty);
				Property("ZipCode", x => x.Address.ZipCode);
				Property("Street", x => x.Address.Street);
				Property("Region", x => x.Address.RegionKey.IsNotNullOrEmpty() ?  regions.FirstOrDefault(c => c.Key == x.Address.RegionKey)?.Value : string.Empty);
				Property("POBox", x => x.Address.POBox);
				Property("ZipCodePOBox", x => x.Address.ZipCodePOBox);
				Property("Phone", x => x.PrimaryPhoneData);
				Property("Fax", x => x.PrimaryFaxData);
				Property("Email", x => x.PrimaryEmailData);
				Property("Website", x => x.Websites.FirstOrDefault());
				Property("ContactType", x => x.ContactType);
				Property("Position", x => appSettingsProvider.GetValue(MainPlugin.Settings.Person.BusinessTitleIsLookup) ? businessTitles.FirstOrDefault(l => l.Key == x.BusinessTitleKey)?.Value : x.BusinessTitleKey);
				Property("Language", x => x.LanguageKey.IsNotNullOrEmpty() ? languages.FirstOrDefault(c => c.Key == x.LanguageKey)?.Value : string.Empty);
				Property("LegacyId", x => x.LegacyId);
				Property("ResponsibleUser", x => x.ResponsibleUserObject != null ? x.ResponsibleUserObject.DisplayName : x.ResponsibleUser);
				Property("CreateDate", x => x.CreateDate.ToShortDateString());
				Property("ModifyDate", x => x.ModifyDate.ToShortDateString());
				Property("Department", x => appSettingsProvider.GetValue(MainPlugin.Settings.Person.DepartmentIsLookup) ? departments.FirstOrDefault(d => d.Key == x.DepartmentTypeKey)?.Value : x.DepartmentTypeKey);
				Property("Tags", x => x.Tags?.ToString(", "));

				//Internal Ids
				Property("SalutationKey", x => x.SalutationKey);
				Property("TitleKey", x => x.TitleKey);
				Property("CountryKey", x => x.Address != null ? x.Address.CountryKey : string.Empty);
				Property("RegionKey", x => x.Address != null ? x.Address.RegionKey : string.Empty);
				Property("LanguageKey", x => x.LanguageKey);

				return base.GetCsv(items);
			}
		}
	}
}
