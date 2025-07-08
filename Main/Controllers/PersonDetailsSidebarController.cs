using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Modularization;
	using Crm.Model;
	using Microsoft.AspNetCore.Authorization;
	using PermissionGroup = MainPlugin.PermissionGroup;

	[Authorize]
	public class PersonDetailsSidebarController : Controller
	{
		[RenderAction("PersonDetailsSidebar", Priority = 80)]
		[RequiredPermission(MainPlugin.PermissionName.SidebarDocumentArchive, Group = PermissionGroup.Person)]
		[RequiredEntityVisibility(typeof(Person))]
		public virtual ActionResult PersonSidebarDocumentArchive()
		{
			return PartialView();
		}
	}
}
