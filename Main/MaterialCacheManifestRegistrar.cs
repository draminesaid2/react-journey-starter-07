namespace Crm
{
	using System.Collections.Generic;

	using Crm.Library.Helper;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Offline;
	using Crm.Library.Offline.Interfaces;
	using Crm.Library.Services.Interfaces;

	public class MaterialCacheManifestRegistrar : CacheManifestRegistrar<MaterialCacheManifest>
	{
		public static Dictionary<string, string> Themes = new Dictionary<string, string>
		{
			{ "pink", "#E91E63" },
			{ "purple", "#9C27B0" },
			{ "blue", "#2196F3" },
			{ "lightblue", "#03A9F4" },
			{ "cyan", "#00BCD4" },
			{ "teal", "#009688" },
			{ "green", "#4CAF50" },
			{ "orange", "#FF9800" },
			{ "bluegray", "#607D8B" }
		};
		private readonly IClientSideGlobalizationService clientSideGlobalizationService;
		private readonly IUserService userService;
		public MaterialCacheManifestRegistrar(IClientSideGlobalizationService clientSideGlobalizationService, IUserService userService, IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			this.clientSideGlobalizationService = clientSideGlobalizationService;
			this.userService = userService;
		}
		protected override void Initialize()
		{
			Cache(WebExtensions.JsLink(null, "cordovaIosJs"));
			Cache(WebExtensions.JsLink(null, "cordovaAndroidJs"));
			Cache(WebExtensions.JsLink(null, "jayDataJs"));
			Cache(WebExtensions.JsLink(null, "loginJs"));
			Cache(WebExtensions.JsLink(null, "materialTs"));
			Cache(WebExtensions.JsLink(null, "materialSystemJs"));
			Cache(WebExtensions.CssLink(null, "loginCss"));
			Cache(WebExtensions.CssLink(null, "loginLess"));
			Cache(WebExtensions.CssLink(null, "materialCss"));
			Cache(WebExtensions.CssLink(null, "templateReportCss"));

			Cache("Account", "UserProfile");
			Cache("Barcode", "Preview");
			Cache("Company", "CreateTemplate");
			Cache("Company", "DetailsTemplate");
			Cache("CompanyList", "FilterTemplate");
			Cache("CompanyList", "IndexTemplate");
			Cache("Contact", "AddTag");
			Cache("Contact", "RenameTagTemplate");
			Cache("Dashboard", "IndexTemplate");
			Cache("Dashboard", "CalendarWidgetTemplate");
			Cache("DocumentArchive", "EditTemplate");
			Cache("DocumentAttributeList", "FilterTemplate");
			Cache("DocumentAttributeList", "IndexTemplate");
			Cache("Error", "Template");
			Cache("Home", "Index");
			Cache("Home", "MaterialIndex");
			Cache("Home", "Startup");
			Cache("Model", "GetDefinitions");
			Cache("Model", "GetRules");
			Cache("NoteList", "IndexTemplate");
			Cache("NoteList", "FilterTemplate");
			Cache("Note", "EditTemplate");
			Cache("Bravo", "EditTemplate");
			Cache("BusinessRelationship", "EditTemplate");
			Cache("CompanyBranch", "EditTemplate");
			Cache("Person", "CreateTemplate");
			Cache("Person", "DetailsTemplate");
			Cache("PersonList", "FilterTemplate");
			Cache("PersonList", "IndexTemplate");
			Cache("Posting", "DetailsTemplate");
			Cache("Posting", "Edit");
			Cache("Posting", "MaterialPostingsTabHeader");
			Cache("Posting", "MaterialPostingsTab");
			Cache("PostingList", "IndexTemplate");
			Cache("PostingList", "FilterTemplate");
			Cache("PostingList", "SkipTransactions");
			Cache("PostingList", "TriggerPostingServiceTopMenu");
			Cache("StationList", "IndexTemplate");
			Cache("StationList", "FilterTemplate");
			Cache("Task", "Edit");
			Cache("Address", "EditTemplate");
			Cache("TaskList", "FilterTemplate");
			Cache("TaskList", "GetIcsLink");
			Cache("TaskList", "IndexTemplate");
			Cache("Template", "DateFilter");
			Cache("Template", "EmptyStateBox");
			Cache("Template", "ContactData");
			Cache("Template", "AddressBlock");
			Cache("Template", "AddressEditor");
			Cache("Template", "AddressSelector");
			Cache("Template", "BarcodeScanner");
			Cache("Template", "FloatingActionButton");
			Cache("Template", "FlotChart");
			Cache("Template", "FormElement");
			Cache("Template", "FullCalendarWidget");
			Cache("Template", "GetRssLink");
			Cache("Template", "InlineEditor");
			Cache("Template", "LvActions");
			Cache("Template", "MiniChart");
			Cache("Template", "PmbBlock");
			Cache("Template", "PmbbEdit");
			Cache("Template", "PmbbEditEntry");
			Cache("Template", "PmbbView");
			Cache("Template", "PmbbViewEntry");
			Cache("Template", "SignaturePad");
			Cache("Template", "Breadcrumbs");
			Cache("Template", "LicensingAlert");
			Cache("Template", "NoteTemplate");
			Cache("Visibility", "Edit");
			Cache("Visibility", "Selection");
			Cache(() => $"~/Authorization/{userService.CurrentUser.Id}.json");
			Cache("~/api/$metadata");
			Cache("~/Content/img/avatar.jpg");
			Cache("~/Content/img/lmobile-block-excited.svg");
			Cache("~/Content/img/lmobile-block-happy.svg");
			Cache("~/Content/img/lmobile-block-sad.svg");
			Cache("~/Content/img/marker_pin3.png");
			Cache("~/Content/img/marker_shadow.png");
			Cache("~/Content/img/app-logo-sales.webp");
			Cache("~/Content/img/app-logo-service.webp");
			Cache("~/favicon.ico");
			Cache("~/static-dist/Main/style/fonts/glyphicons-halflings-regular.woff2");
			Cache("~/static-dist/Main/style/fonts/Material-Design-Iconic-Font.woff2?v=2.2.0");
			Cache("~/static-dist/Main/style/fonts/Material-Design-Iconic-Font.woff?v=2.2.0");
			Cache("~/static-dist/Main/style/fonts/Material-Design-Iconic-Font.ttf?v=2.2.0");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-300.woff");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-300.woff2");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-300italic.woff");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-300italic.woff2");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-600.woff");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-600.woff2");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-600italic.woff");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-600italic.woff2");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-700.woff");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-700.woff2");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-700italic.woff");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-700italic.woff2");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-800.woff");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-800.woff2");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-800italic.woff");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-800italic.woff2");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-italic.woff");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-italic.woff2");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-regular.woff");
			Cache("~/Content/style/fonts/open-sans/open-sans-v17-cyrillic_vietnamese_latin_greek-ext_cyrillic-ext_greek-regular.woff2");
			Cache("~/Content/style/img/icons/search.png");
			Cache("~/static-dist/Main/style/img/icons/search-2.png");
			Cache("~/static-dist/Main/style/img/icons/search-2@2x.png");
			Cache("~/static-dist/Main/style/img/profile-menu.png");
			Cache("~/static-dist/Main/style/img/select.png");
			Cache("~/static-dist/Main/style/img/select@2x.png");
			Cache("~/static-dist/Main/style/select2-spinner.gif");
			Cache("~/static-dist/Main/style/img/icons/pin.png");
			Cache("~/static-dist/Main/style/img/icons/unpin.png");
			Cache("~/Resources.json");
			Cache("~/Settings.json");
			Cache("~/Resource/GetGlobalizationData?format=json");
			Cache("~/Resource/ListLocales?format=json");
			Cache("~/Resource/ListTimeZones?format=json");
			Cache("~/Content/js/system/cldrjs/supplemental/likelySubtags.json");
			Cache("~/Content/js/system/cldrjs/supplemental/currencyData.json");
			Cache("~/Content/js/system/cldrjs/supplemental/timeData.json");
			Cache("~/Content/js/system/cldrjs/supplemental/weekData.json");
			Cache("~/Content/js/system/cldrjs/supplemental/numberingSystems.json");
			Cache("~/Content/js/system/cldrjs/supplemental/plurals.json");
			Cache("~/Content/js/system/cldrjs/supplemental/ordinals.json");
			var cultureResourcePath = clientSideGlobalizationService.GetCurrentCultureOrDefaultResourcePath();
			Cache($"{cultureResourcePath}/ca-gregorian.json");
			Cache($"{cultureResourcePath}/currencies.json");
			Cache($"{cultureResourcePath}/dateFields.json");
			Cache($"{cultureResourcePath}/languages.json");
			Cache($"{cultureResourcePath}/localeDisplayNames.json");
			Cache($"{cultureResourcePath}/numbers.json");
			Cache($"{cultureResourcePath}/scripts.json");
			Cache($"{cultureResourcePath}/territories.json");
			Cache($"{cultureResourcePath}/timeZoneNames.json");
			Cache($"{cultureResourcePath}/variants.json");
		}
	}
}
