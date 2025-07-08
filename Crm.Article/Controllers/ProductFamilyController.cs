namespace Crm.Article.Controllers
{
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ProductFamilyController : Controller
	{
		[RequiredPermission(PermissionName.Create, Group = ArticlePlugin.PermissionGroup.ProductFamily)]
		public virtual ActionResult CreateTemplate()
		{
			return PartialView();
		}

		[RequiredPermission(PermissionName.Read, Group = ArticlePlugin.PermissionGroup.ProductFamily)]
		public virtual ActionResult DetailsTemplate()
		{
			return PartialView();
		}
		[RequiredPermission(PermissionName.Edit, Group = ArticlePlugin.PermissionGroup.ProductFamily)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}
	}
}
