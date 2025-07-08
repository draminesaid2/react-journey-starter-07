using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class VisibilityController : Controller
	{
		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.Visibility)]
		public virtual ActionResult Edit()
		{
			return PartialView();
		}
		
		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.Visibility)]
		public virtual ActionResult Selection()
		{
			return PartialView();
		}
	}
}
