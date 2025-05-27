
using System;
using System.Collections.Generic;
using System.Linq;
using LMobile.MiniForms;
using LMobile.Gen3LicenseManagement.Dao.Contracts;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gen3LicenseManagement.Portal.BusinessObjects;
using LMobile.Gen3LicenseManagement.Portal.Applications.QuestionDialog;

namespace LMobile.Gen3LicenseManagement.Portal.Applications.Modules {
	[AllowRole("Gen3Modules")]
	[AllowAdministratorRoles]
	class ModulesDirectApplication : BaseApplication {
		protected ILicenseDao LicenseDao { get { return this.Session.GetService<ILicenseDao>(); } }
		protected IModuleDao ModuleDao { get { return this.Session.GetService<IModuleDao>(); } }
		
		public List<ModuleProperty> AllPackages;
		public ModuleProperty CurrentPackage;
		
		public void Start() {
			LoadPackages();
			this.DisplayView<ModulesDirectView>();
		}

		public void NavigateEditPackage(int packageID) {
			if (packageID == 0) {
				this.CurrentPackage = new ModuleProperty();
			} else {
				this.CurrentPackage = this.ModuleDao.GetModuleProperty(packageID);
				if (this.CurrentPackage == null) {
					this.LoadPackages();
					throw new Error(Resources.SomebodyElseDeletedTheRecord());
				}
			}
			DisplayView<ModulesDirectView>();
		}
		
		public bool CanUserEditPackage {
			get { return this.Client.CurrentPrincipal.IsInRole("Gen3EditModule"); }
		}
		
		public void ResetCurrentPackage() {
			this.CurrentPackage = null;
			this.LoadPackages();
		}
		
		public void SaveCurrentPackage() {
			if (CurrentPackage.ID == 0) {
				if (string.IsNullOrEmpty(this.CurrentPackage.PropertyName)) { throw new Error("Package name cannot be empty."); }
				if (string.IsNullOrEmpty(this.CurrentPackage.Description)) { throw new Error("Package description cannot be empty."); }
				Session.Insert(CurrentPackage);
				LicenseDao.LogEntry(null, null, MessageTypes.ModulePropertyCreated, "Package '" + CurrentPackage.PropertyName + "' created by '" + Client.CurrentPrincipal.Identity.Name + "'.");
			} else {
				if (!Session.Update(CurrentPackage)) throw new Error(Resources.SomebodyElseModifiedTheRecord());
				LicenseDao.LogEntry(null, null, MessageTypes.ModulePropertyModified, "Package '" + CurrentPackage.PropertyName + "' modified by '" + Client.CurrentPrincipal.Identity.Name + "'.");
			}
			this.CurrentPackage = null;
			this.LoadPackages();
		}

		public bool CanUserDeletePackage {
			get { return this.Client.CurrentPrincipal.IsInRole("Gen3EditModule"); }
		}

		public void ConfirmDeletePackage(int packageID) {
			if (!CanUserDeletePackage)
				return;
			
			var packageToDelete = this.ModuleDao.GetModuleProperty(packageID);
			if (packageToDelete == null) {
				throw new Error(Resources.SomebodyElseDeletedTheRecord());
			}
			
			string confirmationMessage = "Wirklich löschen: " + packageToDelete.PropertyName + "?";
			
			var questionApp = new QuestionApplication(confirmationMessage, (response) => {
				if (response == Responses.YES) {
					DeletePackage(packageID);
				}
			}, QButtons.YES_NO);
			
			questionApp.ShowDialog();
		}

		private bool DeletePackage(int packageID) {
			var packageToDelete = this.ModuleDao.GetModuleProperty(packageID);
			if (packageToDelete == null) {
				throw new Error(Resources.SomebodyElseDeletedTheRecord());
			}
			
			string packageName = packageToDelete.PropertyName;
			if (this.ModuleDao.DeleteModuleProperty(packageID)) {
				LicenseDao.LogEntry(null, null, MessageTypes.ModulePropertyModified, "Package '" + packageName + "' deleted by '" + Client.CurrentPrincipal.Identity.Name + "'.");
				this.LoadPackages();
				return true;
			}
			return false;
		}

		private void LoadPackages() {
			this.AllPackages = this.ModuleDao.GetModuleProperties();
		}
	}
}
