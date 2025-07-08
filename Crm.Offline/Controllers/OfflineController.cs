using Microsoft.AspNetCore.Mvc;

namespace Crm.Offline.Controllers
{
	using System.Collections.Generic;

	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;

	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class OfflineController : Controller
	{
		private readonly IAuthorizationManager authorizationManager;
		private readonly IUserService userService;
		public OfflineController(IAuthorizationManager authorizationManager, IUserService userService)
		{
			this.authorizationManager = authorizationManager;
			this.userService = userService;
		}
		[RenderAction("MaterialTitleResource", Priority = 4890)]
		public virtual ActionResult MaterialHeadResourceAvailableOfflineStatuses()
		{
			var availableOfflineStatuses = new List<string>();
			if (authorizationManager.IsAuthorizedForAction(userService.CurrentUser, PermissionGroup.Login, PermissionName.MaterialClientOffline))
			{
				if (authorizationManager.IsAuthorizedForAction(userService.CurrentUser, PermissionGroup.Login, PermissionName.MaterialClientOnline))
				{
					availableOfflineStatuses.Add("online");
				}
				availableOfflineStatuses.Add("offline");
			} 
			else
			{
				availableOfflineStatuses.Add("online");
			}
			return PartialView("HeadResourceAvailableOfflineStatuses", availableOfflineStatuses.ToArray());
		}
		[RenderAction("MaterialTopMenu", Priority = 10)]
		[RequiredPermission(PermissionName.MaterialClientOffline, Group = PermissionGroup.Login)]
		public virtual ActionResult MaterialTopMenu()
		{
			return PartialView();
		}

		[AllowAnonymous]
		[RenderAction("MaterialHeadResource", Priority = 3990)]
		[RequiredPermission(PermissionName.MaterialClientOffline, Group = PermissionGroup.Login)]
		public virtual ActionResult MaterialHeadResource()
		{
			return Content(Url.JsResource("Crm.Offline", "offlineMaterialJs"));
		}

		[RenderAction("DocumentAttributeAttribute")]
		[RequiredPermission(PermissionName.MaterialClientOffline, Group = PermissionGroup.Login)]
		public virtual ActionResult DocumentAttributeAttribute()
		{
			return PartialView();
		}

		[RequiredPermission(nameof(DocumentAttribute), Group = PermissionGroup.WebApi)]
		[RequiredPermission(nameof(FileResource), Group = PermissionGroup.WebApi)]
		public virtual ActionResult Files()
		{
			return PartialView();
		}

		[RenderAction("Startup")]
		public virtual ActionResult StartupTranslations()
		{
			return PartialView();
		}
		[RenderAction("ListIncompleteWarning")]
		public virtual ActionResult ListIncompleteWarning() => PartialView();
	}
}
