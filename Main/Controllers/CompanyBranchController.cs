namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class CompanyBranchController : Controller
	{
		[RequiredPermission(PermissionName.Edit, Group = MainPlugin.PermissionGroup.Branch)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}
	}
}
