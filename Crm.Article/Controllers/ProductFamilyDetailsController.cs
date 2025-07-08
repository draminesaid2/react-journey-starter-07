namespace Crm.Article.Controllers
{
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ProductFamilyDetailsController : Controller
	{
		[RenderAction("ProductFamilyDetailsMaterialTabHeader", Priority = 100)]
		public virtual ActionResult MaterialDetailsTabHeader()
		{
			return PartialView();
		}

		[RenderAction("ProductFamilyDetailsMaterialTab", Priority = 100)]
		public virtual ActionResult MaterialDetailsTab()
		{
			return PartialView();
		}
	}
}
