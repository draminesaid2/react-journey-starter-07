namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Mvc;


	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class CompanyPersonRelationshipController : Controller
	{
		[RequiredPermission(PermissionName.Edit, Group = MainPlugin.PermissionGroup.CompanyPersonRelationship)]
		public virtual ActionResult EditTemplate() => PartialView();

		[RenderAction("CompanyDetailsRelationshipTypeExtension", Priority = 40)]
		public virtual ActionResult CompanyDetailsRelationshipTypeExtension() => PartialView();

		[RenderAction("CompanyDetailsRelationshipTypeActionExtension", Priority = 40)]
		[RequiredPermission(PermissionName.Edit, Group = MainPlugin.PermissionGroup.CompanyPersonRelationship)]
		public virtual ActionResult CompanyDetailsRelationshipTypeActionExtension() => PartialView();

		[RenderAction("MaterialPersonItemExtensions", Priority = 40)]
		public virtual ActionResult MaterialPersonItemExtensions() => PartialView("MaterialItemExtensions");

		[RenderAction("MaterialCompanyItemExtensions", Priority = 40)]
		public virtual ActionResult MaterialCompanyItemExtensions() => PartialView("MaterialItemExtensions");

		[RenderAction("PersonItemTemplateActions", Priority = 40)]
		public virtual ActionResult PersonItemTemplateActions() => PartialView("MaterialItemTemplateActions");

		[RenderAction("CompanyItemTemplateActions", Priority = 40)]
		public virtual ActionResult CompanyItemTemplateActions() => PartialView("MaterialItemTemplateActions");

		[RenderAction("PersonDetailsRelationshipTypeExtension", Priority = 40)]
		public virtual ActionResult PersonDetailsRelationshipTypeExtension() => PartialView();

		[RenderAction("PersonDetailsRelationshipTypeActionExtension", Priority = 40)]
		[RequiredPermission(PermissionName.Edit, Group = MainPlugin.PermissionGroup.CompanyPersonRelationship)]
		public virtual ActionResult PersonDetailsRelationshipTypeActionExtension() => PartialView();

	}
}
