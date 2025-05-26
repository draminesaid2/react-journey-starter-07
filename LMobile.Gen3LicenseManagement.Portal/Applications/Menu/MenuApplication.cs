
﻿using LMobile.Gen3LicenseManagement.Portal.Applications.Licenses;
using LMobile.Gen3LicenseManagement.Portal.Applications.Roles;
using LMobile.Gen3LicenseManagement.Portal.Applications.Modules;
using LMobile.Gen3LicenseManagement.Portal.Menu;
using LMobile.MiniForms.Classic;

namespace LMobile.Gen3LicenseManagement.Portal.Applications.Menu {
	public class MenuApplication : ClassicMenuApplication {
		public MenuApplication()
		{
			this.Items.Add(new ClassicMenuItem<LicensesApplication>() {
				Caption = Resources.Gen3CustomersMenu(),
				Application = () => new LicensesApplication(),
				Startup = (app, parameter) => app.Start(parameter),
				CausesValidation = false,
			});
			
			this.Items.Add(new ClassicMenuItem<ModulesApplication>() {
				Caption = "Packages", // Changed from "Module" to "Packages"
				Application = () => new ModulesApplication(),
				Startup = (app, parameter) => app.Start(),
				CausesValidation = false,
			});
			
			this.Items.Add(new ClassicMenuItem<ModulesDirectApplication>() {
				Caption = "Modules", // New direct modules interface
				Application = () => new ModulesDirectApplication(),
				Startup = (app, parameter) => app.Start(),
				CausesValidation = false,
			});
		}
		protected override void OnStart() {
			this.DisplayView<MenuView>();
		}
		protected override void OnRoles() {
			this.RunApplication(new RolesApplication(), app => app.Start());
		}
	}
}
