using Microsoft.AspNetCore.Mvc;

namespace Crm.Controllers
{
	using System.Linq;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Menu;
	using Crm.Library.Services.Interfaces;
	using Crm.ViewModels;
	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class MenuController : Controller
	{
		private readonly MenuProvider<MaterialMainMenu> materialMainMenuProvider;
		private readonly MenuProvider<MaterialUserProfileMenu> materialUserProfileMenuProvider;
		private readonly IUserService userService;
		private readonly IAuthorizationManager authorizationManager;

		public virtual ActionResult MaterialMainMenu()
		{
			var user = userService.CurrentUser;
			var menuEntries = materialMainMenuProvider.GetMenuEntriesVisibleToUser(user, authorizationManager).ToList();

			return PartialView("MaterialMenu", new CrmModelList<MenuEntry> { List = menuEntries });
		}
		public virtual ActionResult MaterialUserProfileMenu()
		{
			var user = userService.CurrentUser;
			var menuEntries = materialUserProfileMenuProvider.GetMenuEntriesVisibleToUser(user, authorizationManager).ToList();

			return PartialView("MaterialMenu", new CrmModelList<MenuEntry> { List = menuEntries });
		}
		public MenuController(IUserService userService, MenuProvider<MaterialMainMenu> materialMainMenuProvider, MenuProvider<MaterialUserProfileMenu> materialUserProfileMenuProvider, IAuthorizationManager authorizationManager)
		{
			this.userService = userService;
			this.materialMainMenuProvider = materialMainMenuProvider;
			this.materialUserProfileMenuProvider = materialUserProfileMenuProvider;
			this.authorizationManager = authorizationManager;
		}
	}
}
