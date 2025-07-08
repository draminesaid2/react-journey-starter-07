namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class BravoActionRoleProvider : RoleCollectorBase
	{
		public BravoActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.Bravo, PermissionName.Index, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Bravo, PermissionName.Edit, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Bravo, PermissionName.Edit, MainPlugin.PermissionGroup.Bravo, PermissionName.Create);
			Add(MainPlugin.PermissionGroup.Bravo, PermissionName.Create, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Bravo, PermissionName.Create, MainPlugin.PermissionGroup.Bravo, MainPlugin.PermissionName.Deactivate);
			Add(MainPlugin.PermissionGroup.Bravo, PermissionName.Delete, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Bravo, PermissionName.Delete, MainPlugin.PermissionGroup.Bravo, PermissionName.Edit);
			Add(MainPlugin.PermissionGroup.Bravo, MainPlugin.PermissionName.Deactivate, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Bravo, MainPlugin.PermissionName.Deactivate, MainPlugin.PermissionGroup.Bravo, MainPlugin.PermissionName.Activate);
			Add(MainPlugin.PermissionGroup.Bravo, MainPlugin.PermissionName.Activate, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Bravo, MainPlugin.PermissionName.Activate, MainPlugin.PermissionGroup.Bravo, PermissionName.Index);
		}
	}
}