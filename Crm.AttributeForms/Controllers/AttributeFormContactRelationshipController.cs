using Microsoft.AspNetCore.Mvc;

namespace Crm.AttributeForms.Controllers
{
	using System.Linq;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Modularization;
	using Crm.Library.Services.Interfaces;
	using Crm.Services.Interfaces;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class AttributeFormContactRelationshipController : Controller
	{
		private readonly IContactTypeProvider contactTypeProvider;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly IResourceManager resourceManager;
		private readonly IUserService userService;
		public AttributeFormContactRelationshipController(IContactTypeProvider contactTypeProvider, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, IUserService userService)
		{
			this.contactTypeProvider = contactTypeProvider;
			this.appSettingsProvider = appSettingsProvider;
			this.resourceManager = resourceManager;
			this.userService = userService;
		}
		
		[RenderAction("FormDesignerSidebarTabContent", Priority = 50)]
		public virtual ActionResult FormDesignerSidebarTabContent()
		{
			return PartialView();
		}
		
		[RenderAction("FormDesignerSidebarTabHeader", Priority = 50)]
		public virtual ActionResult FormDesignerSidebarTabHeader()
		{
			return PartialView();
		}

		public virtual ActionResult GetContactTypes()
		{
			var excludedContactTypes = appSettingsProvider.GetValue(AttributeFormsPlugin.Settings.ExcludedContactTypes);
			var userLang = userService.CurrentUser.DefaultLanguageKey;
			var contactTypes = contactTypeProvider.ContactTypes.Except(excludedContactTypes).ToDictionary(x => GetContactType(x), x => (userLang != null ? resourceManager.GetTranslation(x, userLang) : resourceManager.GetTranslation(x)) ?? x).OrderBy(x => x.Value);
			return Json(contactTypes);
		}

		protected virtual string GetContactType(string entityTypeName)
		{
			var typeName = entityTypeName;
			switch (typeName)
			{
				case "ServiceOrderHead":
					typeName = "ServiceOrder";
					break;
			}
			return typeName;
		}

		public virtual ActionResult RelationshipEditor()
		{
			return PartialView();
		}
	}
}
