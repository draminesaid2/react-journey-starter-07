namespace Crm.Article.Controllers
{
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ArticleController : Controller
	{
		[HttpGet]
		[RequiredPermission(PermissionName.Create, Group = ArticlePlugin.PermissionGroup.Article)]
		public virtual ActionResult CreateTemplate()
		{
			return PartialView();
		}

		[AllowAnonymous]
		[RenderAction("MaterialHeadResource", Priority = 2200)]
		public virtual ActionResult MaterialHeadResource()
		{
			return Content(Url.JsResource("Crm.Article", "articleMaterialJs"));
		}
		[RequiredPermission(PermissionName.Read, Group = ArticlePlugin.PermissionGroup.Article)]
		public virtual ActionResult DetailsTemplate()
		{
			return PartialView();
		}
		[RenderAction("ArticleDetailsMaterialTabHeader", Priority = 60)]
		[RequiredPermission(MainPlugin.PermissionName.DocumentsTab, Group = ArticlePlugin.PermissionGroup.Article)]
		public virtual ActionResult MaterialDocumentsTabHeader()
		{
			return PartialView("ContactDetails/MaterialDocumentsTabHeader");
		}

		[RenderAction("ArticleDetailsMaterialTab", Priority = 60)]
		[RequiredPermission(MainPlugin.PermissionName.DocumentsTab, Group = ArticlePlugin.PermissionGroup.Article)]
		public virtual ActionResult MaterialDocumentsTab()
		{
			return PartialView("ContactDetails/MaterialDocumentsTab");
		}
		[RenderAction("ArticleDetailsMaterialTab", Priority = 80)]
		[RequiredPermission(MainPlugin.PermissionName.RelationshipsTab, Group = ArticlePlugin.PermissionGroup.Article)]
		public virtual ActionResult RelationshipsTab()
		{
			return PartialView();
		}
		[RenderAction("ArticleDetailsMaterialTabHeader", Priority = 80)]
		[RequiredPermission(MainPlugin.PermissionName.RelationshipsTab, Group = ArticlePlugin.PermissionGroup.Article)]
		public virtual ActionResult RelationshipsTabHeader()
		{
			return PartialView();
		}
	}
}
