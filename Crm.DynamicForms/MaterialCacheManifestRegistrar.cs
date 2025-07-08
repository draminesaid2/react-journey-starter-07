namespace Crm.DynamicForms
{
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Offline;
	using Crm.Library.Offline.Interfaces;
	using Crm.Library.Services.Interfaces;

	using static Crm.DynamicForms.DynamicFormsPlugin;

	public class MaterialCacheManifestRegistrar : CacheManifestRegistrar<MaterialCacheManifest>
	{
		private readonly IAuthorizationManager authorizationManager;
		private readonly IUserService userService;
		public MaterialCacheManifestRegistrar(IPluginProvider pluginProvider, IAuthorizationManager authorizationManager, IUserService userService)
			: base(pluginProvider)
		{
			this.authorizationManager = authorizationManager;
			this.userService = userService;
		}
		protected override void Initialize()
		{
			var rootPath = "~/static-dist/Crm.DynamicForms/style/";
			CacheJs("dynamicFormsMaterialJs");
			CacheJs("dynamicFormsMaterialTs");
			CacheJs("dynamicFormsResponseJs");
			CacheCss("dynamicFormsCss");
			CacheCss("dynamicFormsMaterialCss");
			Cache($"{rootPath}/fonts/fontawesome-webfont.eot");
			Cache($"{rootPath}/fonts/fontawesome-webfont.svg");
			Cache($"{rootPath}/fonts/fontawesome-webfont.ttf");
			Cache($"{rootPath}/fonts/fontawesome-webfont.woff");

			if (authorizationManager.IsAuthorizedForAction(userService.CurrentUser, PermissionGroup.PdfFeature, PermissionName.Read))
			{
				CacheJs("dynamicFormsViewerJs");
				CacheCss("dynamicFormsViewerCss");
				Cache("~/Plugins/Crm.DynamicForms/Content/js/pdfjs/web/locale/locale.properties");
				Cache("~/Plugins/Crm.DynamicForms/Content/js/pdfjs/web/locale/de/viewer.properties");
				Cache("~/Plugins/Crm.DynamicForms/Content/js/pdfjs/web/locale/en-GB/viewer.properties");
				Cache("~/Plugins/Crm.DynamicForms/Content/js/pdfjs/web/locale/en-US/viewer.properties");
				Cache("~/Plugins/Crm.DynamicForms/Content/js/pdfjs/web/locale/es-ES/viewer.properties");
				Cache("~/Plugins/Crm.DynamicForms/Content/js/pdfjs/web/locale/hu/viewer.properties");
				Cache("~/Plugins/Crm.DynamicForms/Content/js/pdfjs/web/locale/fr/viewer.properties");
				Cache("~/Plugins/Crm.DynamicForms/Content/js/pdfjs/web/Fehler.pdf");
				Cache("~/Plugins/Crm.DynamicForms/Content/js/pdfjs/build/pdf.sandbox.js");
				Cache("~/Plugins/Crm.DynamicForms/Content/js/pdfjs/build/pdf.worker.js");
				Cache($"{rootPath}/images/loading-icon.gif");
				Cache($"{rootPath}/images/shadow.png");
			}
		}
	}
}
