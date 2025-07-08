namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Model;

	public class DeviceActionRoleProvider : RoleCollectorBase
	{
		public DeviceActionRoleProvider(IPluginProvider pluginProvider) : base(pluginProvider)
		{
			var roles = new[] { MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice };

			Add(PermissionGroup.WebApi, nameof(Device), roles);
		}
	}
}
