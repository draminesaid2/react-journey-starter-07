using Microsoft.AspNetCore.Mvc;

namespace Crm.Article.Controllers
{
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class ArticleCompanyRelationshipController : Controller
	{

		[RenderAction("CompanyDetailsRelationshipTypeExtension", Priority = 70)]
		public virtual ActionResult CompanyDetailsRelationshipTypeExtension() => PartialView();

		
		[RenderAction("CompanyDetailsRelationshipTypeActionExtension", Priority = 50)]
		public virtual ActionResult CompanyDetailsRelationshipTypeActionExtension() => PartialView();
		
		
		[RenderAction("MaterialArticleItemExtensions", Priority = 50)]
		public virtual ActionResult MaterialProjectItemExtensions() => PartialView("MaterialItemExtensions");
		
		[RenderAction("MaterialCompanyItemExtensions", Priority = 60)]
		public virtual ActionResult MaterialCompanyItemExtensions() => PartialView("MaterialItemExtensions");

		[RenderAction("ArticleItemTemplateActions", Priority = 50)]
		public virtual ActionResult ArticleItemTemplateActions() => PartialView("MaterialItemTemplateActions");
		
		[RenderAction("CompanyItemTemplateActions", Priority = 20)]
		public virtual ActionResult CompanyItemTemplateActions() => PartialView("MaterialItemTemplateActions");
		
		public virtual ActionResult EditTemplate() => PartialView("../Relationship/EditTemplate");
	}
}
