using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Modularization;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class TemplateController : Controller
	{
		public virtual ActionResult AddressBlock()
		{
			return PartialView();
		}
		public virtual ActionResult AddressEditor()
		{
			return PartialView();
		}
		public virtual ActionResult AddressSelector()
		{
			return PartialView();
		}
		public virtual ActionResult BarcodeScanner()
		{
			return PartialView();
		}
		[RequiredPermission("RecentPage", Group = PermissionGroup.WebApi)]
		public virtual ActionResult Breadcrumbs()
		{
			return PartialView();
		}
		
		public virtual ActionResult DateFilter()
		{
			return PartialView();
		}
		public virtual ActionResult EmptyStateBox()
		{
			return PartialView();
		}
		public virtual ActionResult ContactData()
		{
			return PartialView();
		}
		public virtual ActionResult FloatingActionButton()
		{
			return PartialView();
		}
		public virtual ActionResult FlotChart()
		{
			return PartialView();
		}
		public virtual ActionResult FormElement()
		{
			return PartialView();
		}
		public virtual ActionResult FullCalendarWidget()
		{
			return PartialView();
		}		
		public virtual ActionResult GetRssLink()
		{
			return PartialView();
		}
		public virtual ActionResult InlineEditor()
		{
			return PartialView("InlineEdit/InlineEditor");
		}
		public virtual ActionResult LicensingAlert()
		{
			return PartialView();
		}
		public virtual ActionResult LvActions()
		{
			return PartialView();
		}

		public virtual ActionResult MiniChart()
		{
			return PartialView();
		}

		public virtual ActionResult NoteTemplate()
		{
			return PartialView();
		}

		public virtual ActionResult PmbBlock()
		{
			return PartialView("InlineEdit/PmbBlock");
		}
		public virtual ActionResult PmbbEdit()
		{
			return PartialView("InlineEdit/PmbbEdit");
		}
		public virtual ActionResult PmbbEditEntry()
		{
			return PartialView("InlineEdit/PmbbEditEntry");
		}
		public virtual ActionResult PmbbView()
		{
			return PartialView("InlineEdit/PmbbView");
		}
		public virtual ActionResult PmbbViewEntry()
		{
			return PartialView("InlineEdit/PmbbViewEntry");
		}
		public virtual ActionResult ScaleFilter()
		{
			return PartialView();
		}
		public virtual ActionResult SignaturePad()
		{
			return PartialView("SignaturePad");
		}
		[AllowAnonymous]
		[RenderAction("MaterialTitleResource", "TemplateHeadResource", Priority = 50)]
		public virtual ActionResult TemplateReportCss()
		{
			return Content(Url.CssResource("templateReportCss"));
		}
		[AllowAnonymous]
		[RenderAction("TemplateHeadResource", Priority = 10000)]
		public virtual ActionResult TemplateReportJs()
		{
			return Content(Url.JsResource("templateReportJs"));
		}
		[AllowAnonymous]
		[RenderAction("TemplateHeadResource", Priority = 60)]
		public virtual ActionResult TemplateReportMaterialCss()
		{
			return Content(Url.CssResource("materialCss"));
		}
	}
}
