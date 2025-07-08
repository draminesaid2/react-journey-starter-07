namespace Crm.Controllers.ActionRoleProvider
{
    using Crm.Library.Model;
    using Crm.Library.Model.Authorization;
    using Crm.Library.Model.Authorization.PermissionIntegration;
    using Crm.Library.Modularization.Interfaces;

    public class RecentPagesActionRoleProvider : RoleCollectorBase
    {
        protected static readonly string[] Roles = { MainPlugin.Roles.FieldSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice };

        public RecentPagesActionRoleProvider(IPluginProvider pluginProvider)
            : base(pluginProvider)
        {
            Add(PermissionGroup.WebApi, nameof(RecentPage), Roles);
        }
    }
}