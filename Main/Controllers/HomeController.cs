using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.MobileViewEngine;
	using Crm.Library.Services.Interfaces;
	using Crm.Services.Interfaces;
	using Crm.ViewModels;
	using Microsoft.AspNetCore.Authorization;
	[Authorize]
	public class HomeController : Controller
	{
		private readonly IUserService userService;
		private readonly IBrowserCapabilities browserCapabilities;
		private readonly IEnumerable<IRedirectProvider> redirectProviders;
		public HomeController(IUserService userService, IBrowserCapabilities browserCapabilities, IEnumerable<IRedirectProvider> redirectProviders)
		{
			this.userService = userService;
			this.browserCapabilities = browserCapabilities;
			this.redirectProviders = redirectProviders;
		}

		[AllowAnonymous]
		public virtual ActionResult Index()
		{
			if (userService.CurrentUser == null)
			{
				return new RedirectResult("~/Account/Login");
			}

			var redirects = redirectProviders.Select(x => x.Index(userService.CurrentUser, browserCapabilities)).Where(x => x != null).Distinct().ToArray();
			if (!redirects.Any())
			{
				return ClientSelection();
			}
			if (redirects.Length == 1)
			{
				var redirect = redirects.First();
				Request.RouteValues["plugin"] = redirect.Plugin;
				Request.RouteValues["controller"] = redirect.Controller;
				Request.RouteValues["action"] = redirect.Action;
				return redirect.ActionResult.Invoke(this);
			}
			return ClientSelection();
		}
		public virtual ActionResult ClientSelection()
		{
			var redirects = redirectProviders.Select(x => x.Index(userService.CurrentUser, browserCapabilities)).Where(x => x != null).Distinct().ToArray();
			var model = new ClientSelectionViewModel
			{
				RedirectProviderResults = redirects.ToList()
			};
			return View("ClientSelection", model);
		}
		public virtual ActionResult MaterialIndex()
		{
			var model = new CrmModel();
			if (!Request.Query.ContainsKey("token"))
			{
				model.CacheManifest = "material.appcache";
			}
			return View("MaterialIndex", model);
		}
		public virtual ActionResult Startup()
		{
			return View("Startup", new CrmModel());
		}
		public virtual ActionResult MaterialTopMenu() => PartialView();
	}
}
