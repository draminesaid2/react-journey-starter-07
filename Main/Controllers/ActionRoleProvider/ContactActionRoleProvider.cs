namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class ContactActionRoleProvider : RoleCollectorBase
	{
		public ContactActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.Contact, MainPlugin.PermissionName.ExportAsCsv, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.Contact, MainPlugin.PermissionName.Merge, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.Contact, MainPlugin.PermissionName.SidebarTasks, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);

			Add(PermissionGroup.Contact, MainPlugin.PermissionName.CreateTag, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.Contact, MainPlugin.PermissionName.AssociateTag, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(PermissionGroup.Contact, MainPlugin.PermissionName.RemoveTag, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
		}
	}
}