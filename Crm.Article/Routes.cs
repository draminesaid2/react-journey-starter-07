namespace Crm.Article
{

	using Crm.Library.Modularization.Registrars;
	using Crm.Library.Rest;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Routing;

	public class Routes : IRouteRegistrar
	{
		public virtual RoutePriority Priority
		{
			get { return RoutePriority.AboveNormal; }
		}
		public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapControllerRoute(
				null,
				"Crm.Article/{controller}/{action}.{format}",
				new { plugin = "Crm.Article" },
				new { format = new IsJsonOrXml(), controller = new RestControllerName(), plugin = "Crm.Article" }
			);

			endpoints.MapControllerRoute(
				null,
				"Crm.Article/{controller}/{action}/{id?}",
				new { action = "Index", plugin = "Crm.Article" },
				new { plugin = "Crm.Article" }
			);
		}
	}
}
