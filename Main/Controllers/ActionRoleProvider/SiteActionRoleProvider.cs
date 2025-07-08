namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization.Interfaces;

	using PermissionGroup = MainPlugin.PermissionGroup;

	public class SiteActionRoleProvider : RoleCollectorBase
	{
		public SiteActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(PermissionGroup.Site, PermissionName.Settings);
			Add(PermissionGroup.Site, PermissionName.SetTheme);
			Add(PermissionGroup.Site, PermissionName.SetProperties);
			Add(PermissionGroup.Site, PermissionName.SaveReportSettings);
			Add(Crm.Library.Model.Authorization.PermissionGroup.WebApi, nameof(Site), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(Crm.Library.Model.Authorization.PermissionGroup.WebApi, PermissionName.Settings);
		}
	}
}
