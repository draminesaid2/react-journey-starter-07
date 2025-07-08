namespace Crm.Offline.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class OfflineActionRoleProvider : RoleCollectorBase
	{
		public OfflineActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.Login, PermissionName.MaterialClientOffline, MainPlugin.Roles.FieldSales, "FieldService");
		}
	}
}
