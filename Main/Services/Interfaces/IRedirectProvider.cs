using Microsoft.AspNetCore.Mvc;

namespace Crm.Services.Interfaces
{

	using Crm.Library.AutoFac;
	using Crm.Library.MobileViewEngine;
	using Crm.Library.Model;

	public interface IRedirectProvider : IDependency
	{
		RedirectProviderResult Index(User user, IBrowserCapabilities browserCapabilities);
		ActionResult RedirectAfterLogin(User user, IBrowserCapabilities browserCapabilities, string returnUrl);
		string AvailableClients(User user);
	}
}