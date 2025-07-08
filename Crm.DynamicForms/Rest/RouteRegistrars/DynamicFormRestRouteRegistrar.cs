namespace Crm.DynamicForms.Rest.RouteRegistrars
{

	using Crm.Library.Modularization.Registrars;
	using Crm.Library.Rest;

	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.AspNetCore.Routing.Constraints;

	public class DynamicFormRestRouteRegistrar : IRouteRegistrar
	{
		public virtual RoutePriority Priority
		{
			get { return RoutePriority.Important; }
		}
		public virtual void RegisterRoutes(IEndpointRouteBuilder endpoints)
		{
			endpoints.MapControllerRoute(
								null,
				"Crm.DynamicForms/FormElementTypes.{format}",
								new { plugin = "Crm.DynamicForms", controller = "DynamicFormElementRest", action = "ListElementTypes" },
								new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("GET"), plugin = "Crm.DynamicForms" }
								);

			endpoints.MapControllerRoute(
								null,
				"Crm.DynamicForms/SaveFormReference.{format}",
								new { plugin = "Crm.DynamicForms", controller = "DynamicFormRest", action = "SaveFormReference" },
								new { format = new IsJsonOrXml(), httpMethod = new HttpMethodRouteConstraint("POST"), plugin = "Crm.DynamicForms" }
								);
		}
	}
}
