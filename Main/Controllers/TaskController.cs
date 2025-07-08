namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class TaskController : Controller
	{
		[RequiredPermission(PermissionName.Edit, Group = MainPlugin.PermissionGroup.Task)]
		public virtual ActionResult Edit()
		{
			return PartialView();
		}

		[RenderAction("MaterialTopMenu", Priority = 5)]
		[RequiredPermission(PermissionName.Index, Group = MainPlugin.PermissionGroup.Task)]
		public virtual ActionResult MaterialTopMenu()
		{
			return PartialView();
		}
	}
}
