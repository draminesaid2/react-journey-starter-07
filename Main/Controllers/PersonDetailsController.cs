using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Modularization;
	using Crm.Model;
	using Microsoft.AspNetCore.Authorization;
	using PermissionGroup = MainPlugin.PermissionGroup;

	[Authorize]
	public class PersonDetailsController : Controller
	{
		[RenderAction("MaterialPersonSidebarExtensions", Priority = 50)]
		public virtual ActionResult DropboxBlock()
		{
			return PartialView("ContactDetailsDropboxBlock", typeof(Person));
		}

		[RenderAction("PersonDetailsMaterialTabHeader", Priority = 100)]
		public virtual ActionResult MaterialDetailsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("PersonDetailsMaterialTab", Priority = 100)]
		public virtual ActionResult MaterialDetailsTab()
		{
			return PartialView();
		}

		[RenderAction("PersonDetailsMaterialTabHeader", Priority = 60)]
		[RequiredPermission(MainPlugin.PermissionName.DocumentsTab, Group = PermissionGroup.Person)]
		public virtual ActionResult MaterialDocumentsTabHeader()
		{
			return PartialView("ContactDetails/MaterialDocumentsTabHeader");
		}

		[RenderAction("PersonDetailsMaterialTab", Priority = 60)]
		[RequiredPermission(MainPlugin.PermissionName.DocumentsTab, Group = PermissionGroup.Person)]
		public virtual ActionResult MaterialDocumentsTab()
		{
			return PartialView("ContactDetails/MaterialDocumentsTab");
		}

		[RenderAction("PersonDetailsMaterialTabHeader", Priority = 90)]
		[RequiredPermission(MainPlugin.PermissionName.NotesTab, Group = PermissionGroup.Person)]
		public virtual ActionResult MaterialNotesTabHeader()
		{
			return PartialView();
		}

		[RenderAction("PersonDetailsMaterialTab", Priority = 90)]
		[RequiredPermission(MainPlugin.PermissionName.NotesTab, Group = PermissionGroup.Person)]
		public virtual ActionResult MaterialNotesTab()
		{
			return PartialView();
		}

		[RenderAction("PersonDetailsMaterialTabHeader", Priority = 70)]
		[RequiredPermission(MainPlugin.PermissionName.StaffTab, Group = PermissionGroup.Person)]
		public virtual ActionResult MaterialStaffTabHeader()
		{
			return PartialView();
		}

		[RenderAction("PersonDetailsMaterialTab", Priority = 70)]
		[RequiredPermission(MainPlugin.PermissionName.StaffTab, Group = PermissionGroup.Person)]
		public virtual ActionResult MaterialStaffTab()
		{
			return PartialView();
		}

		[RenderAction("PersonDetailsMaterialTabHeader", Priority = 80)]
		[RequiredPermission(MainPlugin.PermissionName.TasksTab, Group = PermissionGroup.Person)]
		public virtual ActionResult MaterialTasksTabHeader()
		{
			return PartialView();
		}
		[RenderAction("PersonDetailsMaterialTab", Priority = 80)]
		[RequiredPermission(MainPlugin.PermissionName.TasksTab, Group = PermissionGroup.Person)]
		public virtual ActionResult MaterialTasksTab()
		{
			return PartialView();
		}
	}
}
