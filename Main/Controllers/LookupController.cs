using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class LookupController : Controller
	{
		private readonly ILookupService lookupService;

		[RequiredPermission(MainPlugin.PermissionName.Lookup, Group = PermissionGroup.WebApi)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}

		[RequiredRole(RoleName.Administrator)]
		public virtual ActionResult RefreshLookupCache()
		{
			lookupService.PurgeCache();
			return RedirectToAction("Index", "Lookup");
		}
		public LookupController(ILookupService lookupService)
		{
			this.lookupService = lookupService;
		}
	}
}
