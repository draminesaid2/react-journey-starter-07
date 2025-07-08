namespace Crm.Offline
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Menu;

	public class OfflineMenuRegistrar : IMenuRegistrar<MaterialUserProfileMenu>
	{
		public virtual void Initialize(MenuProvider<MaterialUserProfileMenu> menuProvider)
		{
			menuProvider.Register(null, "DataSynchronization", url: "~/Main/Home/Startup?sync=true", iconClass: "zmdi zmdi-refresh-sync", priority: Int32.MaxValue - 10, htmlAttributes: new Dictionary<string, object> { { "id", "menu-sync" }, { "style", "display: none;" }, { "data-bind", "visible: window.Helper.Offline.status === 'offline'" } });
			menuProvider.AddPermission(null, "DataSynchronization", PermissionGroup.Login, PermissionName.MaterialClientOffline);
			menuProvider.Register(null, "MaterialClientOnlineLoginTitle", url: "~/Main/Home/Startup/online", iconClass: "zmdi zmdi-home", priority: Int32.MaxValue - 20, htmlAttributes: new Dictionary<string, object> { { "style", "display: none;" }, { "data-bind", "visible: window.Helper.Offline.status === 'offline'" } });
			menuProvider.AddPermission(null, "MaterialClientOnlineLoginTitle", PermissionGroup.Login, PermissionName.MaterialClientOnline);
			menuProvider.AddPermission(null, "MaterialClientOnlineLoginTitle", PermissionGroup.Login, PermissionName.MaterialClientOffline);
			menuProvider.Register(null, "MaterialClientOfflineLoginTitle", url: "~/Main/Home/Startup/offline", iconClass: "zmdi zmdi-airplane", priority: Int32.MaxValue - 20, htmlAttributes: new Dictionary<string, object> { { "style", "display: none;" }, { "data-bind", "visible: window.Helper.Offline.status === 'online'" } });
			menuProvider.AddPermission(null, "MaterialClientOfflineLoginTitle", PermissionGroup.Login, PermissionName.MaterialClientOnline);
			menuProvider.AddPermission(null, "MaterialClientOfflineLoginTitle", PermissionGroup.Login, PermissionName.MaterialClientOffline);
		}
	}
}
