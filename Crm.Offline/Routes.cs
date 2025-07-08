namespace Crm.Offline
{

	using Crm.Library.Modularization.Registrars;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.AspNetCore.Routing.Constraints;

	public class Routes : IRouteRegistrar
	{
		public virtual RoutePriority Priority
		{
			get { return RoutePriority.AboveNormal; }
		}
		public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapControllerRoute(
				name: null,
				"Crm.Offline/Sync/{action}",
				defaults: new { controller = "Sync", plugin = "Crm.Offline", action = "GetAll" },
				new { httpMethod = new HttpMethodRouteConstraint("GET") }
			);
			endpoints.MapControllerRoute(
				name: null,
				"Crm.Offline/Sync/{action}",
				defaults: new { controller = "Sync", plugin = "Crm.Offline", action = "Save" },
				new { httpMethod = new HttpMethodRouteConstraint("POST", "PUT") }
			);
			endpoints.MapControllerRoute(
				name: null,
				"Crm.Offline/Sync/{action}",
				defaults: new { controller = "Sync", plugin = "Crm.Offline", action = "Remove" },
				new { httpMethod = new HttpMethodRouteConstraint("DELETE") }
			);
			endpoints.MapControllerRoute(
				null,
				"Crm.Offline/{controller}/{action}/{id?}",
				new { action = "Index", plugin = "Crm.Offline" },
				new { plugin = "Crm.Offline" }
				);
		}
	}
}
