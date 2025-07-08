namespace Crm.AttributeForms
{

	using Crm.Library.Modularization.Registrars;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Routing;

	public class Routes : IRouteRegistrar
	{
		public virtual RoutePriority Priority => RoutePriority.Normal;
		public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
		{
			// Add additional routes here

			endpoints.MapControllerRoute(
				null,
				"Crm.AttributeForms/{controller}/{action}/{id?}",
				new { action = "Index", plugin = "Crm.AttributeForms" },
				new { plugin = "Crm.AttributeForms" }
			);
		}
	}
}
