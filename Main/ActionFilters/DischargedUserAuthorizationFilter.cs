namespace Crm.ActionFilters
{
	using Crm.Library.ActionFilterRegistry;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.Mvc.Filters;

	public class DischargedUserAuthorizationFilter : ICrmAuthorizationFilter
	{
		private readonly IAuthenticationService authenticationService;
		private readonly IUserService userService;
		public DischargedUserAuthorizationFilter(IAuthenticationService authenticationService, IUserService userService)
		{
			this.authenticationService = authenticationService;
			this.userService = userService;
		}
		public virtual void OnAuthorization(AuthorizationFilterContext filterContext)
		{
			if (userService.CurrentUser != null && userService.CurrentUser.Discharged)
			{
				authenticationService.SignOut();
				filterContext.HttpContext.Response.Redirect("~/Account/Login", false);
			}
		}
	}
}
