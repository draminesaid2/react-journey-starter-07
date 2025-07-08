namespace Crm.Article.Controllers;

using Crm.Library.Modularization;
using Microsoft.AspNetCore.Mvc;

public class ArticleGroupController : Controller
{
	[RenderAction("LookupEditBasicInformation")]
	public virtual ActionResult LookupPropertyEditImage() => PartialView();
}