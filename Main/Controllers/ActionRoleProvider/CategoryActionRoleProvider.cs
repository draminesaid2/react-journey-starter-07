namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class CategoryActionRoleProvider : RoleCollectorBase
	{
		public CategoryActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.Category, PermissionName.Index, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Category, PermissionName.Delete, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Category, PermissionName.Edit, MainPlugin.Roles.SalesBackOffice);
		}
	}
}