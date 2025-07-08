using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;
	using PermissionGroup = MainPlugin.PermissionGroup;

	[Authorize]
	public class RelationshipController : Controller
	{
		[RenderAction("CompanyDetailsMaterialTabHeader", Priority = 30)]
		[RequiredPermission(MainPlugin.PermissionName.RelationshipsTab, Group = PermissionGroup.Company)]
		public virtual ActionResult CompanyDetailsMaterialTabHeader() => PartialView();

		[RenderAction("CompanyDetailsMaterialTab", Priority = 30)]
		[RequiredPermission(MainPlugin.PermissionName.RelationshipsTab, Group = PermissionGroup.Company)]
		public virtual ActionResult CompanyDetailsMaterialTab() => PartialView();

		[RenderAction("MaterialCompanyItemExtensions", Priority = 50)]
		[RequiredPermission(MainPlugin.PermissionName.RelationshipsTab, Group = PermissionGroup.Company)]
		public virtual ActionResult MaterialCompanyItemExtensions() => PartialView();

		[RenderAction("CompanyItemTemplateActions", Priority = 50)]
		[RequiredPermission(MainPlugin.PermissionName.RelationshipsTab, Group = PermissionGroup.Company)]
		public virtual ActionResult CompanyItemTemplateActions() => PartialView();

		[RenderAction("PersonDetailsMaterialTabHeader", Priority = 70)]
		[RequiredPermission(MainPlugin.PermissionName.RelationshipsTab, Group = PermissionGroup.Person)]
		public virtual ActionResult PersonDetailsMaterialTabHeader() => PartialView();

		[RenderAction("PersonDetailsMaterialTab", Priority = 70)]
		[RequiredPermission(MainPlugin.PermissionName.RelationshipsTab, Group = PermissionGroup.Person)]
		public virtual ActionResult PersonDetailsMaterialTab() => PartialView();
	}
}
