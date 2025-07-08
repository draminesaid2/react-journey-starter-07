using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class BravoController : Controller
	{
		[RequiredPermission(PermissionName.Edit, Group = MainPlugin.PermissionGroup.Bravo)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}
	}
}
