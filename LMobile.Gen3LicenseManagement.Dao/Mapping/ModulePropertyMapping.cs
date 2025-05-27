using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gida;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class ModulePropertyMapping : GidaViewMapping {
		public readonly StoredModulePropertyMapping Main;
		public ModulePropertyMapping(GidaSession session)
			: base(session) {
			var define = this.Define<ModuleProperty>();
			this.Main = this.From<StoredModulePropertyMapping>();
			define.SelectIntoResult(this.Main);
		}
	}
}
