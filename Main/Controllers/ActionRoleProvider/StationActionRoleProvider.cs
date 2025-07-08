namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using System.Linq;

	public class StationActionRoleProvider : RoleCollectorBase
	{
		public StationActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var roles = new[] {
				MainPlugin.Roles.SalesBackOffice,
				MainPlugin.Roles.HeadOfSales,
				MainPlugin.Roles.InternalSales,
				MainPlugin.Roles.FieldSales
			};

			Add(nameof(Station), PermissionName.Create);
			Add(nameof(Station), PermissionName.Edit);
			Add(nameof(Station), PermissionName.Index);
			Add(nameof(Station), PermissionName.View);
			Add(PermissionGroup.WebApi, nameof(Station), roles);

			if (pluginProvider.ActivePluginNames.Contains("Crm.Offline"))
			{
				Add(PermissionGroup.Sync, nameof(Station), new[] {MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales });
				AddImport(PermissionGroup.Sync, nameof(Station), PermissionGroup.WebApi, nameof(Station));
			}
		}
	}
}