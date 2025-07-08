namespace Crm.Article.Controllers.ActionRoleProvider
{
	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Article.Model.Relationships;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Model.Lookups;

	public class ArticleActionRoleProvider : RoleCollectorBase
	{
		public ArticleActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(ArticlePlugin.PermissionGroup.Article, PermissionName.View, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			Add(ArticlePlugin.PermissionGroup.Article, PermissionName.Read, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			Add(ArticlePlugin.PermissionGroup.Article, PermissionName.Index, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			Add(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.RelationshipsTab, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Index, ArticlePlugin.PermissionGroup.Article, PermissionName.Read);
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Index, PermissionGroup.WebApi, nameof(ArticleGroup01));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Index, PermissionGroup.WebApi, nameof(ArticleGroup02));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Index, PermissionGroup.WebApi, nameof(ArticleGroup03));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Index, PermissionGroup.WebApi, nameof(ArticleGroup04));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Index, PermissionGroup.WebApi, nameof(ArticleGroup05));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Index, PermissionGroup.WebApi, nameof(ArticleType));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Index, PermissionGroup.WebApi, nameof(Currency));
			Add(ArticlePlugin.PermissionGroup.Article, PermissionName.Create, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales);
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Create, ArticlePlugin.PermissionGroup.Article, PermissionName.Index);
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Create, PermissionGroup.WebApi, nameof(ArticleGroup01));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Create, PermissionGroup.WebApi, nameof(ArticleGroup02));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Create, PermissionGroup.WebApi, nameof(ArticleGroup03));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Create, PermissionGroup.WebApi, nameof(ArticleGroup04));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Create, PermissionGroup.WebApi, nameof(ArticleGroup05));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Create, PermissionGroup.WebApi, nameof(ArticleType));
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Create, PermissionGroup.WebApi, nameof(VATLevel));
			Add(ArticlePlugin.PermissionGroup.Article, PermissionName.Edit, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales);
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Edit, ArticlePlugin.PermissionGroup.Article, PermissionName.Create);
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Edit, ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Create);
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Edit, ArticlePlugin.PermissionGroup.ArticleRelationship, PermissionName.Edit);
			Add(ArticlePlugin.PermissionGroup.Article, PermissionName.Delete, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales);
			AddImport(ArticlePlugin.PermissionGroup.Article, PermissionName.Delete, ArticlePlugin.PermissionGroup.Article, PermissionName.Edit);

			Add(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.CreateTag, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales);
			AddImport(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.CreateTag, ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.AssociateTag);
			Add(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.AssociateTag, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales);
			Add(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.RenameTag, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales);
			AddImport(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.RenameTag, ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.DeleteTag);
			Add(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.RemoveTag, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales);
			AddImport(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.RemoveTag, ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.AssociateTag);
			Add(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.DeleteTag, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales);
			AddImport(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.DeleteTag, ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.CreateTag);

			Add(PermissionGroup.WebApi, nameof(Article));
			AddImport(PermissionGroup.WebApi, nameof(Article), ArticlePlugin.PermissionGroup.Article, PermissionName.Read);
			Add(PermissionGroup.WebApi, nameof(ArticleDescription));
			Add(PermissionGroup.WebApi, nameof(ArticleGroup01));
			Add(PermissionGroup.WebApi, nameof(ArticleGroup02));
			Add(PermissionGroup.WebApi, nameof(ArticleGroup03));
			Add(PermissionGroup.WebApi, nameof(ArticleGroup04));
			Add(PermissionGroup.WebApi, nameof(ArticleGroup05));
			Add(PermissionGroup.WebApi, nameof(ArticleType));
			Add(PermissionGroup.WebApi, nameof(QuantityUnit));
			Add(PermissionGroup.WebApi, nameof(VATLevel)); 
			Add(PermissionGroup.WebApi, nameof(Article), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ArticleType), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ArticleDescription), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ArticleGroup01), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ArticleGroup02), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ArticleGroup03), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ArticleGroup04), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ArticleGroup05), MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(QuantityUnit), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ArticleCompanyRelationship), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(ArticleCompanyRelationshipType), MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice, Roles.APIUser);
			AddImport(PermissionGroup.WebApi, nameof(ArticleCompanyRelationship), ArticlePlugin.PermissionGroup.Article, PermissionName.Read);
			AddImport(PermissionGroup.WebApi, nameof(Article), PermissionGroup.WebApi, nameof(ArticleCompanyRelationship));
			AddImport(PermissionGroup.WebApi, nameof(ArticleCompanyRelationshipType), ArticlePlugin.PermissionGroup.Article, PermissionName.Read);
			AddImport(PermissionGroup.WebApi, nameof(Article), PermissionGroup.WebApi, nameof(ArticleCompanyRelationshipType));
			AddImport(MainPlugin.PermissionGroup.Company, MainPlugin.PermissionName.RelationshipsTab, ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.RelationshipsTab);
			Add(ArticlePlugin.PermissionGroup.Article, MainPlugin.PermissionName.DocumentsTab, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
		}
	}
}
