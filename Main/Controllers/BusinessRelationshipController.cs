using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class BusinessRelationshipController : Controller
	{
		[RequiredPermission(PermissionName.Edit, Group = MainPlugin.PermissionGroup.BusinessRelationship)]
		public virtual ActionResult EditTemplate() => PartialView("../Relationship/EditTemplate");
	}
}
