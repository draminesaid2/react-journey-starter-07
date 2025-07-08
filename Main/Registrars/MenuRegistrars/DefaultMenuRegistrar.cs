namespace Crm.Registrars.MenuRegistrars
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Menu;
	using Crm.Model;
	using Crm.Rest.Model;

	public class DefaultMenuRegistrar : IMenuRegistrar<MaterialMainMenu>, IMenuRegistrar<MaterialUserProfileMenu>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void Initialize(MenuProvider<MaterialUserProfileMenu> menuProvider)
		{
			menuProvider.Register(null, "Settings", url: "~/Main/Account/UserProfile", iconClass: "zmdi zmdi-account-circle", priority: Int32.MaxValue - 100);
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void Initialize(MenuProvider<MaterialMainMenu> menuProvider)
		{
			menuProvider.Register(null, "Dashboard", url: "~/Main/Dashboard/IndexTemplate", iconClass: "zmdi zmdi-home", priority: Int32.MaxValue - 10);
			menuProvider.AddPermission(null, "Dashboard", PermissionGroup.MaterialDashboard, PermissionName.View);
			menuProvider.Register(null, "Notes", url: "~/Main/NoteList/IndexTemplate", iconClass: "zmdi zmdi-comment-text", priority: Int32.MaxValue - 15);
			menuProvider.AddPermission(null, "Notes", MainPlugin.PermissionGroup.Note, PermissionName.View);

			menuProvider.Register(null, "ContactsTitle", iconClass: "zmdi zmdi-accounts-alt", priority: Int32.MaxValue - 30);
			menuProvider.Register("ContactsTitle", "CompaniesTitle", url: "~/Main/CompanyList/IndexTemplate", priority: Int32.MaxValue - 10);
			menuProvider.AddPermission("ContactsTitle", "CompaniesTitle", MainPlugin.PermissionGroup.Company, PermissionName.View);
			menuProvider.Register("ContactsTitle", "PeopleTitle", url: "~/Main/PersonList/IndexTemplate", priority: Int32.MaxValue - 20);
			menuProvider.AddPermission("ContactsTitle", "PeopleTitle", MainPlugin.PermissionGroup.Person, PermissionName.View);

			menuProvider.Register(null, "TasksTitle", iconClass: "zmdi zmdi-assignment-check", priority: Int32.MaxValue - 40);
			menuProvider.AddPermission(null, "TasksTitle", MainPlugin.PermissionGroup.Task, PermissionName.View);
			menuProvider.Register("TasksTitle", "OpenTasks", url: "~/Main/TaskList/IndexTemplate?completedTasks=false", priority: Int32.MaxValue - 10);
			menuProvider.Register("TasksTitle", "CompletedTasks", url: "~/Main/TaskList/IndexTemplate?completedTasks=true", priority: Int32.MaxValue - 20);

			menuProvider.Register(null, "DocumentAttributes", iconClass: "zmdi zmdi-info-outline", url: "~/Main/DocumentAttributeList/IndexTemplate", priority: Int32.MaxValue - 90);
			menuProvider.AddPermission(null, "DocumentAttributes", MainPlugin.PermissionGroup.DocumentAttribute, PermissionName.View);
			
			menuProvider.Register(null, "MasterData",  iconClass: "zmdi zmdi-layers", priority: -95);
			menuProvider.Register("MasterData", "Stations", url: "~/Main/StationList/IndexTemplate", priority: 60);
			menuProvider.AddPermission("MasterData", "Stations", nameof(Station), PermissionName.View);

			menuProvider.Register(null, "Administration", iconClass: "zmdi zmdi-settings", priority: -100, htmlAttributes: new Dictionary<string, object> { { "data-bind", "if: !window.Helper.Offline || window.Helper.Offline.status == 'online'" } });
			menuProvider.Register("Administration", "UserAdministrationTitle", url: "~/Main/UserList/IndexTemplate", priority: 90);
			menuProvider.Register("Administration", "UserGroups", url: "~/Main/UserGroupList/IndexTemplate", iconClass: "zmdi zmdi-alert-circle-o", priority: 85);
			menuProvider.AddPermission("Administration", "UserGroups", PermissionGroup.WebApi, nameof(Usergroup));
			menuProvider.AddPermission("Administration", "UserAdministrationTitle", PermissionGroup.UserAdmin, PermissionName.Index);
			menuProvider.Register("Administration", "RolesAndPermissionsTitle", url: "~/Main/PermissionSchemaRoleList/IndexTemplate", priority: 80);
			menuProvider.AddPermission("Administration", "RolesAndPermissionsTitle", PermissionGroup.UserAdmin, MainPlugin.PermissionName.ListRoles);
			menuProvider.Register("Administration", "BackgroundServices", iconClass: "zmdi zmdi-alarm zmdi-hc-fw", url: "~/Main/BackgroundService/IndexTemplate", priority: 70);
			menuProvider.AddPermission("Administration", "BackgroundServices", PermissionGroup.WebApi, nameof(MainPlugin.PermissionGroup.BackgroundService));
			menuProvider.Register("Administration", "Logs", url: "~/Main/LogList/IndexTemplate", iconClass: "zmdi zmdi-alert-circle-o", priority: 60);
			menuProvider.AddPermission("Administration", "Logs", PermissionGroup.WebApi, nameof(Log));
			menuProvider.Register("Administration", "Transactions", iconClass: "zmdi zmdi-view-list", url: "~/Main/PostingList/IndexTemplate", priority: 50);
			menuProvider.AddPermission("Administration", "Transactions", PermissionGroup.WebApi, nameof(Posting));
			menuProvider.Register("Administration", "EmailMessages", iconClass: "zmdi zmdi-email", url: "~/Main/MessageList/IndexTemplate", priority: 40);
			menuProvider.AddPermission("Administration", "EmailMessages", PermissionGroup.WebApi, nameof(Message));
			menuProvider.Register("Administration", "DocumentGeneration", iconClass: "zmdi zmdi-email", url: "~/Main/DocumentGeneratorList/IndexTemplate", priority: 30);
			menuProvider.AddPermission("Administration", "DocumentGeneration", PermissionGroup.WebApi, nameof(DocumentGeneratorEntry));
			menuProvider.Register("Administration", "Lookups", url: "~/Main/LookupList/IndexTemplate", priority: 20);
			menuProvider.AddPermission("Administration", "Lookups", PermissionGroup.WebApi, MainPlugin.PermissionName.Lookup);
			menuProvider.Register("Administration", "SiteSettings", url: "~/Main/Site/Details", priority: -120);
			menuProvider.AddPermission("Administration", "SiteSettings", MainPlugin.PermissionGroup.Site, PermissionName.Settings);
			menuProvider.AddPermission("Administration", "SiteSettings", PermissionGroup.WebApi, PermissionName.Settings);

			menuProvider.Register(null, "SwitchMode", iconClass: "zmdi zmdi-power-setting", priority: -100);
		}
	}
}
