using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gida;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	class ModulePropertiesInModulesMapping : GidaViewMapping {
		public readonly StoredModulePropertiesInModulesMapping Main;
		public ModulePropertiesInModulesMapping(GidaSession session)
			: base(session) {
			var define = this.Define<ModulePropertiesInModules>();
			this.Main = this.From<StoredModulePropertiesInModulesMapping>();
			define.SelectIntoResult(this.Main);
		}
	}
}
