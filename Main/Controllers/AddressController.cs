namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class AddressController : Controller
	{
		[RequiredPermission(PermissionName.Edit, Group = MainPlugin.PermissionGroup.Address)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}
	}
}
