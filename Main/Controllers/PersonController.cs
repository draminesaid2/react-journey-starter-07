using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;
	using PermissionGroup = MainPlugin.PermissionGroup;

	[Authorize]
	public class PersonController : Controller
	{
		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.Person)]
		public virtual ActionResult CreateTemplate()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.Person)]
		public virtual ActionResult DetailsTemplate()
		{
			return PartialView();
		}
		
		[RequiredPermission(MainPlugin.PermissionName.Merge, Group = PermissionGroup.Person)]
		public virtual ActionResult Merge()
		{
			return PartialView();
		}

		[RenderAction("PersonDetailsTopMenu")]
		public virtual ActionResult TopMenu()
		{
			return PartialView();
		}
	}
}
