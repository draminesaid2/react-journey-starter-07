namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class AddressActionRoleProvider : RoleCollectorBase
	{
		public AddressActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.Address, PermissionName.Create, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Address, PermissionName.Edit, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Address, PermissionName.Edit, MainPlugin.PermissionGroup.Address, PermissionName.Create);
			AddImport(MainPlugin.PermissionGroup.Address, PermissionName.Edit, MainPlugin.PermissionGroup.Address, MainPlugin.PermissionName.ExportAsVcf);
			Add(MainPlugin.PermissionGroup.Address, PermissionName.Delete, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Address, PermissionName.Delete, MainPlugin.PermissionGroup.Address, PermissionName.Edit);
			Add(MainPlugin.PermissionGroup.Address, MainPlugin.PermissionName.ExportAsVcf, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
		}
	}
}