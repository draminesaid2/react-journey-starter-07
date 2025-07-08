using Crm.Library.Model.Authorization.PermissionIntegration;

namespace Crm.Article.Controllers.ActionRoleProvider
{
	using Crm.Article.Model.Lookups;
	using Crm.Article.Model.Relationships;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Modularization.Interfaces;

	public class ArticleRelationshipActionRoleProvider : RoleCollectorBase
	{
		public ArticleRelationshipActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Create, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales);
			AddImport(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Create, ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Read);
			Add(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Edit, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales);
			AddImport(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Edit, ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Read);
			Add(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Delete, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales);
			AddImport(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Delete, ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Create);
			AddImport(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Delete, ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Edit);
			Add(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Read, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			AddImport(ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Read, ArticlePlugin.PermissionGroup.Article, PermissionName.Read);

			Add(PermissionGroup.WebApi, nameof(ArticleRelationship), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ArticleRelationshipType), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales, Roles.APIUser);
		}
	}
}