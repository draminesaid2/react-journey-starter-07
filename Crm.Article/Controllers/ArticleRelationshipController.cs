namespace Crm.Article.Controllers
{
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ArticleRelationshipController : Controller
	{
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}
		[RenderAction("ArticleItemTemplateActions")]
		public virtual ActionResult ItemTemplateAction()
		{
			return PartialView();
		}

		[RenderAction("MaterialArticleItemExtensions", Priority = 50)]
		public virtual ActionResult MaterialArticleItemExtensions() => PartialView("MaterialItemExtensions");
	}
}
