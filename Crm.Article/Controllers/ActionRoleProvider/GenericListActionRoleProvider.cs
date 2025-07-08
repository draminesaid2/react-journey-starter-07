namespace Crm.Article.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class GenericListActionRoleProvider : RoleCollectorBase
	{
		public GenericListActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.ExportAsCsv, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			Add(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.Ics, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			Add(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.Rss, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
		}
	}
}