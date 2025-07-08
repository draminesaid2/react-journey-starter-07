using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Microsoft.AspNetCore.Authorization;
	using PermissionGroup = MainPlugin.PermissionGroup;

	[Authorize]
	public class NoteController : Controller
	{
		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.Note)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}
	}
}
