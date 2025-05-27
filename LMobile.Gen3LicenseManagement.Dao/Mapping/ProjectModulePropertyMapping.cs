using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gida;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class ProjectModulePropertyMapping : GidaViewMapping {
		public readonly StoredProjectModulePropertyMapping Main;
		public readonly ModulePropertyMapping ModuleProperty;
		public ProjectModulePropertyMapping(GidaSession session)
			: base(session) {
			var define = this.Define<ProjectModuleProperty>();
			this.Main = this.From<StoredProjectModulePropertyMapping>();
			define.SelectIntoResult(this.Main);
			this.ModuleProperty = this.MergedInnerJoin<ModulePropertyMapping>(c => c.Main.ID == this.Main.ModulePropertyID);
			define.Select<ModuleProperty>(this.ModuleProperty, (o, value) => o.ModuleProperty = value);
		}
	}
}
