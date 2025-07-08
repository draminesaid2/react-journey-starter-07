namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class CompanyDetailsActionRoleProvider : RoleCollectorBase
	{
		public CompanyDetailsActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.DocumentsTab, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.DocumentsTab, MainPlugin.PermissionGroup.Company, PermissionName.Read);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.UpgradeProspect, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales);
			AddImport(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.UpgradeProspect, MainPlugin.PermissionGroup.Company, PermissionName.Edit);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.EditAddress, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.FieldSales);
			AddImport(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.EditAddress, MainPlugin.PermissionGroup.Company, PermissionName.Read);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.DeleteAddress, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.FieldSales);
			AddImport(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.DeleteAddress, MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.EditAddress);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.DeleteCommunication, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.FieldSales);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.CreateBusinessRelationship, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.DeleteBusinessRelationship, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);

			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.SidebarBackgroundInfo, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.SidebarBravo, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.SidebarClientCompanies, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.SidebarContactInfo, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.SidebarStaffList, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.SidebarDocumentArchive, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);

			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.RelationshipsTab, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.NotesTab, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.StaffTab, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.TasksTab, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
		}
	}
}