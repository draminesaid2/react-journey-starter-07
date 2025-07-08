namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class PersonActionRoleProvider : RoleCollectorBase
	{
		public PersonActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.Person, PermissionName.View, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, PermissionName.Index, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, PermissionName.Read, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, PermissionName.Create, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Person, PermissionName.Create, MainPlugin.PermissionGroup.Person, PermissionName.Read);
			Add(MainPlugin.PermissionGroup.Person, PermissionName.Edit, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Person, PermissionName.Edit, MainPlugin.PermissionGroup.Person, PermissionName.Create);
			Add(MainPlugin.PermissionGroup.Person, PermissionName.Delete, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Person, PermissionName.Delete, MainPlugin.PermissionGroup.Person, PermissionName.Edit);

			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.Merge, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.AddTask, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.ImportFromVcf, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);

			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.NotesTab, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.TasksTab, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.StaffTab, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.RelationshipsTab, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.DocumentsTab, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);

			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.CreateTag, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.CreateTag, MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.AssociateTag);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.AssociateTag, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.RenameTag, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.RenameTag, MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.DeleteTag);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.RemoveTag, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.RemoveTag, MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.AssociateTag);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.DeleteTag, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.DeleteTag, MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.CreateTag);

			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.PublicHeaderInfo, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.PrivateHeaderInfo, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.SidebarBackgroundInfo, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.SidebarBravo, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.SidebarContactInfo, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.SidebarStaffList, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.SidebarDocumentArchive, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);

			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.DeleteBusinessRelationship, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.DeleteCommunication, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.MakeStandardAddress, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.EditAddress, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Person, MainPlugin.PermissionName.DeleteAddress, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
		}
	}
}