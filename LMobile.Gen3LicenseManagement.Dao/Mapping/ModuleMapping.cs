using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gida;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class ModuleMapping : GidaViewMapping {
		public readonly StoredModuleMapping Main;
		public readonly CustomerMapping Customer;
		public readonly GidaColumnMapping InstallationCount;
		public ModuleMapping(GidaSession session)
			: base(session) {
			var define = this.Define<Module>();
			this.Main = this.From<StoredModuleMapping>();
			define.SelectIntoResult(this.Main);
		}
	}
}
