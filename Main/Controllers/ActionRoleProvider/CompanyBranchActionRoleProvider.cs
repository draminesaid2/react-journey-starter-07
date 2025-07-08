namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class CompanyBranchActionRoleProvider : RoleCollectorBase
	{
		public CompanyBranchActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.Branch, PermissionName.Edit, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
		}
	}
}