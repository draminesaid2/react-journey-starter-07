using System;
using System.Collections.Generic;
namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public class Project : StoredProject {
		public virtual Customer Customer { get; set; }
		public virtual int InstallationCount { get; set; }
		public virtual List<ProjectModuleProperty> ModuleProperties { get; set; }
	}
}
