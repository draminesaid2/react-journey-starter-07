using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class UserController : Controller
	{
		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult Create() => PartialView();
		[RequiredPermission(MainPlugin.PermissionName.UserDetail, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult DetailsTemplate() => PartialView();
		[RenderAction("UserDetailsMaterialTabHeader")]
		public virtual ActionResult DetailsTabHeader() => PartialView();
		[RenderAction("UserDetailsMaterialTab")]
		public virtual ActionResult DetailsTab() => PartialView();
		public virtual ActionResult ResetPassword() => PartialView();
	}
}
