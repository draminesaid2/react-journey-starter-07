namespace Crm.Controllers
{
	using Crm.Library.Helper;
	using Crm.Library.Modularization;

	using Microsoft.AspNetCore.Mvc;

	using Newtonsoft.Json;

	public class PushNotificationController : Controller
	{
		private readonly IAppSettingsProvider appSettingsProvider;
		
		[RenderAction("MaterialContactSidebarExtensions", Priority = 100)]
		public virtual ActionResult PushNotificationContactSidebarExtension() => PartialView();

		public virtual ActionResult GetConfiguration()
		{
			return new JsonResult(JsonConvert.DeserializeObject(appSettingsProvider.GetValue(MainPlugin.Settings.PushNotification.Configuration)));
		}
		
		public PushNotificationController(IAppSettingsProvider appSettingsProvider)
		{
			this.appSettingsProvider = appSettingsProvider;
		}
	}
}
