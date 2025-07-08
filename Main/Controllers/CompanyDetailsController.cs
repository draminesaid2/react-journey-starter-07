using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Modularization;
	using Crm.Model;
	using Crm.ViewModels;
	using Microsoft.AspNetCore.Authorization;
	using PermissionGroup = MainPlugin.PermissionGroup;

	[Authorize]
	public class CompanyDetailsController : Controller
	{
		[RenderAction("MaterialCompanySidebarExtensions", Priority = 50)]
		public virtual ActionResult DropboxBlock()
		{
			return PartialView("ContactDetailsDropboxBlock", typeof(Company));
		}

		[RenderAction("CompanyDetailsMaterialTabHeader", Priority = 100)]
		public virtual ActionResult MaterialDetailsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("CompanyDetailsMaterialTab", Priority = 100)]
		public virtual ActionResult MaterialDetailsTab()
		{
			return PartialView();
		}

		[RenderAction("CompanyDetailsMaterialTabHeader", Priority = 60)]
		[RequiredPermission(MainPlugin.PermissionName.DocumentsTab, Group = PermissionGroup.Company)]
		public virtual ActionResult MaterialDocumentsTabHeader()
		{
			return PartialView("ContactDetails/MaterialDocumentsTabHeader");
		}

		[RenderAction("CompanyDetailsMaterialTab", Priority = 60)]
		[RequiredPermission(MainPlugin.PermissionName.DocumentsTab, Group = PermissionGroup.Company)]
		public virtual ActionResult MaterialDocumentsTab()
		{
			return PartialView("ContactDetails/MaterialDocumentsTab");
		}

		[RenderAction("CompanyDetailsMaterialTabHeader", Priority = 90)]
		[RequiredPermission(MainPlugin.PermissionName.NotesTab, Group = PermissionGroup.Company)]
		public virtual ActionResult MaterialNotesTabHeader()
		{
			return PartialView();
		}
		[RenderAction("CompanyDetailsMaterialTab", Priority = 90)]
		[RequiredPermission(MainPlugin.PermissionName.NotesTab, Group = PermissionGroup.Company)]
		public virtual ActionResult MaterialNotesTab()
		{
			return PartialView();
		}

		[RenderAction("CompanyDetailsMaterialTabHeader", Priority = 80)]
		[RequiredPermission(MainPlugin.PermissionName.TasksTab, Group = PermissionGroup.Company)]
		public virtual ActionResult MaterialTasksTabHeader()
		{
			return PartialView();
		}
		[RenderAction("CompanyDetailsMaterialTab", Priority = 80)]
		[RequiredPermission(MainPlugin.PermissionName.TasksTab, Group = PermissionGroup.Company)]
		public virtual ActionResult MaterialTasksTab()
		{
			return PartialView(new CrmModel());
		}

		[RenderAction("CompanyDetailsMaterialTabHeader", Priority = 70)]
		[RequiredPermission(MainPlugin.PermissionName.StaffTab, Group = PermissionGroup.Company)]
		public virtual ActionResult MaterialStaffTabHeader()
		{
			return PartialView();
		}
		[RenderAction("CompanyDetailsMaterialTab", Priority = 70)]
		[RequiredPermission(MainPlugin.PermissionName.StaffTab, Group = PermissionGroup.Company)]
		public virtual ActionResult MaterialStaffTab()
		{
			return PartialView(new CrmModel());
		}
	}
}
