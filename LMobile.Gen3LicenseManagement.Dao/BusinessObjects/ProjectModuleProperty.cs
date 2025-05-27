using System;
using System.Collections.Generic;
namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public class ProjectModuleProperty : StoredProjectModuleProperty {
		public virtual ModuleProperty ModuleProperty { get; set; }
	}
}
