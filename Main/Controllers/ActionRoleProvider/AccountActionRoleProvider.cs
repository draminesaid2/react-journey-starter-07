namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class AccountActionRoleProvider : RoleCollectorBase
	{
		public AccountActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.UserAccount, MainPlugin.PermissionName.ExportDropboxAddressAsVCard, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales);
			Add(MainPlugin.PermissionGroup.UserAccount, PermissionName.Settings, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales);
			Add(MainPlugin.PermissionGroup.UserAccount, MainPlugin.PermissionName.UpdateStatus, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales);
		}
	}
}