using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class DashboardController : Controller
	{
		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.MaterialDashboard)]
		public virtual ActionResult IndexTemplate() {
			return PartialView();
		}

		[RenderAction("Dashboard", Priority = 100)]
		[RequiredPermission(PermissionName.Index, Group = MainPlugin.PermissionGroup.Company)]
		public virtual ActionResult ContactSearchWidget() {
			return PartialView();
		}

		[RenderAction("Dashboard", Priority = 50)]
		[RequiredPermission(PermissionName.Calendar, Group = PermissionGroup.MaterialDashboard)]
		public virtual ActionResult CalendarWidget() {
			return PartialView();
		}

		[RequiredPermission(PermissionName.Calendar, Group = PermissionGroup.MaterialDashboard)]
		public virtual ActionResult CalendarWidgetTemplate() {
			return PartialView();
		}
	}
}
