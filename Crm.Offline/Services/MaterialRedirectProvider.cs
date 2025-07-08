namespace Crm.Offline.Services
{
	using Crm.Library.MobileViewEngine;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Services;
	using Crm.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc;

	public class MaterialOfflineRedirectProvider : IRedirectProvider
	{
		private readonly IAuthorizationManager authorizationManager;
		public MaterialOfflineRedirectProvider(IAuthorizationManager authorizationManager)
		{
			this.authorizationManager = authorizationManager;
		}

		public virtual string AvailableClients(User user)
		{
			if (authorizationManager.IsAuthorizedForAction(user, PermissionGroup.Login, PermissionName.MaterialClientOffline))
			{
				return PermissionName.MaterialClientOffline;
			}
			return null;
		}

		public virtual RedirectProviderResult Index(User user, IBrowserCapabilities browserCapabilities)
		{
			if (authorizationManager.IsAuthorizedForAction(user, PermissionGroup.Login, PermissionName.MaterialClientOffline))
			{
				return new RedirectProviderResult
				{
					Name = "MaterialClientOffline",
					Plugin = "Main",
					Controller = "Home",
					Action = "MaterialIndex",
					ActionResult = x => x.MaterialIndex(),
					Hash = "/Main/Home/Startup/offline",
					Icon = "airplane"
				};
			}
			return null;
		}
		public virtual ActionResult RedirectAfterLogin(User user, IBrowserCapabilities browserCapabilities, string returnUrl)
		{
			if (authorizationManager.IsAuthorizedForAction(user, PermissionGroup.Login, PermissionName.MaterialClientOffline))
			{
				return new RedirectResult("~/Home/MaterialIndex");
			}
			return null;
		}
	}
}
