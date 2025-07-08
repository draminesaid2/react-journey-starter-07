using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;


	[Authorize]
	public class DocumentArchiveController : Controller
	{
		[RequiredPermission(PermissionName.Create, Group = MainPlugin.PermissionGroup.DocumentAttribute)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}
		
		[RenderAction("DocumentsTabPrimaryAction")]
		public virtual ActionResult MaterialDocumentsTabPrimaryAction()
		{
			return PartialView();
		}
	}
}
