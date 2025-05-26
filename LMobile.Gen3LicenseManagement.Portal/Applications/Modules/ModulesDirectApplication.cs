
﻿using System;
using System.Collections.Generic;
using System.Linq;
using LMobile.MiniForms;
using LMobile.Gen3LicenseManagement.Dao.Contracts;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;

namespace LMobile.Gen3LicenseManagement.Portal.Applications.Modules {
	[AllowRole("Gen3Modules")]
	[AllowAdministratorRoles]
	class ModulesDirectApplication : BaseApplication {
		protected ILicenseDao LicenseDao { get { return this.Session.GetService<ILicenseDao>(); } }
		protected IModuleDao ModuleDao { get { return this.Session.GetService<IModuleDao>(); } }
		
		public List<Module> AllModules;
		public List<StoredProjectType> ProjectTypes;
		public Module CurrentModule;
		
		public void Start() {
			this.ProjectTypes = LicenseDao.GetProjectTypes();
			LoadModules();
			this.DisplayView<ModulesDirectView>();
		}

		public void NavigateEditModule(int moduleID) {
			if (moduleID == 0) {
				this.CurrentModule = new Module() {
					ModuleGuid = System.Guid.NewGuid(),
				};
			} else {
				this.CurrentModule = this.ModuleDao.GetModule(moduleID);
				if (this.CurrentModule == null) {
					this.LoadModules();
					throw new Error(Resources.SomebodyElseDeletedTheRecord());
				}
			}
			DisplayView<ModulesDirectView>();
		}
		
		public bool CanUserEditModule {
			get { return this.Client.CurrentPrincipal.IsInRole("Gen3EditModule"); }
		}
		
		public void ResetCurrentModule() {
			this.CurrentModule = null;
			this.LoadModules();
		}
		
		public void SaveCurrentModule() {
			if (CurrentModule.ID == 0) {
				if (string.IsNullOrEmpty(this.CurrentModule.ProjectType)) { throw new Error(Resources.ProjectTypeEmpty()); }
				if (string.IsNullOrEmpty(this.CurrentModule.Description)) { throw new Error(Resources.DescriptionEmpty()); }
				Session.Insert(CurrentModule);
				LicenseDao.LogEntry(null, null, MessageTypes.ModuleCreated, "Module '" + CurrentModule.Description + "' created by '" + Client.CurrentPrincipal.Identity.Name + "'.");
			} else {
				if (!Session.Update(CurrentModule)) throw new Error(Resources.SomebodyElseModifiedTheRecord());
				LicenseDao.LogEntry(null, null, MessageTypes.ModuleModified, "Module '" + CurrentModule.Description + "' modified by '" + Client.CurrentPrincipal.Identity.Name + "'.");
			}
			this.CurrentModule = null;
			this.LoadModules();
		}

		public bool CanUserDeleteModule {
			get { return this.Client.CurrentPrincipal.IsInRole("Gen3EditModule"); }
		}

		public bool DeleteModule(int moduleID) {
			if (!CanUserDeleteModule)
				return false;
			
			var moduleToDelete = this.ModuleDao.GetModule(moduleID);
			if (moduleToDelete == null) {
				throw new Error(Resources.SomebodyElseDeletedTheRecord());
			}
			
			string moduleName = moduleToDelete.Description;
			if (this.ModuleDao.DeleteModule(moduleID)) {
				LicenseDao.LogEntry(null, null, MessageTypes.ModuleModified, "Module '" + moduleName + "' deleted by '" + Client.CurrentPrincipal.Identity.Name + "'.");
				this.LoadModules();
				return true;
			}
			return false;
		}

		private void LoadModules() {
			this.AllModules = this.ModuleDao.GetModules();
		}
	}
}
