using Microsoft.AspNetCore.Mvc;

namespace Crm.Offline.Controllers
{

	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Model.Lookups;
	using Main.Replication.Model;

	public class AccountController : Controller
	{
		[RenderAction("UserProfileTab", Priority = 80)]
		[RequiredPermission(nameof(ReplicationGroup), Group = PermissionGroup.WebApi)]
		[RequiredPermission(nameof(ReplicationGroupSetting), Group = PermissionGroup.WebApi)]
		public virtual ActionResult ReplicationGroupsTab()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTabHeader", Priority = 80)]
		[RequiredPermission(nameof(ReplicationGroup), Group = PermissionGroup.WebApi)]
		[RequiredPermission(nameof(ReplicationGroupSetting), Group = PermissionGroup.WebApi)]
		public virtual ActionResult ReplicationGroupsTabHeader()
		{
			return PartialView();
		}
	}
}
