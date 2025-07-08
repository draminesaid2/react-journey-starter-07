namespace Crm.Article
{
	using System.Runtime.CompilerServices;

	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Menu;

	public class ArticleMenuRegistrar : IMenuRegistrar<MaterialMainMenu>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void Initialize(MenuProvider<MaterialMainMenu> menuProvider)
		{
			menuProvider.Register(
				"MasterData",
				"Articles",
				url: "~/Crm.Article/ArticleList/IndexTemplate",
				iconClass: "zmdi zmdi-file-text",
				priority: 100);
			menuProvider.AddPermission("MasterData", "Articles", ArticlePlugin.PermissionGroup.Article, PermissionName.View);
			menuProvider.Register(
				"MasterData",
				"ProductFamilies",
				url: "~/Crm.Article/ProductFamilyList/IndexTemplate",
				iconClass: "zmdi zmdi-view-dashboard",
				priority: 90);
			menuProvider.AddPermission("MasterData", "ProductFamilies", ArticlePlugin.PermissionGroup.ProductFamily, PermissionName.View);
		}
	}
}
 
