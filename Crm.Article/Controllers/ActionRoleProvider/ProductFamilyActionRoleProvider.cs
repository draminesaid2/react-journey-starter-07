namespace Crm.Article.Controllers.ActionRoleProvider
{
	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class ProductFamilyActionRoleProvider : RoleCollectorBase
	{
		public ProductFamilyActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ArticlePlugin.PermissionGroup.ProductFamily, PermissionName.Index, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			Add(ArticlePlugin.PermissionGroup.ProductFamily, PermissionName.View, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			Add(ArticlePlugin.PermissionGroup.ProductFamily, PermissionName.Create, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales);
			Add(ArticlePlugin.PermissionGroup.ProductFamily, PermissionName.Edit, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales);
			Add(ArticlePlugin.PermissionGroup.ProductFamily, PermissionName.Read, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);

			Add(PermissionGroup.WebApi, nameof(ProductFamily), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales, "HeadOfService", "ServiceBackOffice", "InternalService", Roles.APIUser);
			
			AddImport(ArticlePlugin.PermissionGroup.ProductFamily, PermissionName.Index, PermissionGroup.WebApi, nameof(ProductFamilyStatus));
			AddImport(ArticlePlugin.PermissionGroup.ProductFamily, PermissionName.Create, ArticlePlugin.PermissionGroup.ProductFamily, PermissionName.Index);
			Add(PermissionGroup.WebApi, nameof(ProductFamilyDescription), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			Add(PermissionGroup.WebApi, nameof(ProductFamilyStatus));
		}
	}
}
