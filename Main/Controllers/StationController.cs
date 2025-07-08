namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class StationController : Controller
	{
		[RequiredPermission(PermissionName.Edit, Group = nameof(Station))]
		public virtual ActionResult Assign() => PartialView();

		[RequiredPermission(PermissionName.Create, Group = nameof(Station))]
		public virtual ActionResult Create() => PartialView();

		[RequiredPermission(PermissionName.View, Group = nameof(Station))]
		public virtual ActionResult Details() => PartialView();

		[RenderAction("StationDetailsTab", Priority = 100)]
		public virtual ActionResult DetailsTab() => PartialView();

		[RenderAction("StationDetailsTabHeader", Priority = 100)]
		public virtual ActionResult DetailsTabHeader() => PartialView();

		[RenderAction("StationDetailsTab", Priority = 90)]
		public virtual ActionResult UsersTab() => PartialView();

		[RenderAction("StationDetailsTabHeader", Priority = 90)]
		public virtual ActionResult UsersTabHeader() => PartialView();
	}
}
