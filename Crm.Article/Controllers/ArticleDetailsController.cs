namespace Crm.Article.Controllers
{
	using Crm.Library.Helper;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ArticleDetailsController : Controller
	{
		[RenderAction("MaterialHeadResource", Priority = 2200)]
		public virtual ActionResult MaterialHeadResource()
		{
			return Content(Url.JsResource("Crm.Article", "articleMaterialTs"));
		}

		[RenderAction("ArticleDetailsMaterialTab", Priority = 100)]
		public virtual ActionResult MaterialDetailsTab()
		{
			return PartialView();
		}

		[RenderAction("ArticleDetailsMaterialTabHeader", Priority = 100)]
		public virtual ActionResult MaterialDetailsTabHeader()
		{
			return PartialView();
		}
	}
}
