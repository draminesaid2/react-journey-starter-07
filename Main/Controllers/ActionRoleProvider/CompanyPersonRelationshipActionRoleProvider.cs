namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class CompanyPersonRelationshipActionRoleProvider : RoleCollectorBase
	{
		public CompanyPersonRelationshipActionRoleProvider(IPluginProvider pluginProvider) : base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.CompanyPersonRelationship, PermissionName.Edit, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
		}
	}
}
