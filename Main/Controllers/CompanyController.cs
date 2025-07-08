using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class CompanyController : Controller
	{
		[RequiredPermission(MainPlugin.PermissionName.Merge, Group = MainPlugin.PermissionGroup.Company)]
		public virtual ActionResult Merge()
		{
			return PartialView();
		}

		[RenderAction("CompanyDetailsTopMenu")]
		public virtual ActionResult TopMenu()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Read, Group = MainPlugin.PermissionGroup.Company)]
		public virtual ActionResult DetailsTemplate()
		{
			return PartialView();
		}

		[HttpGet]
		[RequiredPermission(PermissionName.Create, Group = MainPlugin.PermissionGroup.Company)]
		public virtual ActionResult CreateTemplate()
		{
			return PartialView();
		}
	}
}
