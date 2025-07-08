namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class BusinessRelationshipActionRoleProvider : RoleCollectorBase
	{
		public BusinessRelationshipActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.BusinessRelationship, PermissionName.Edit, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
		}
	}
}