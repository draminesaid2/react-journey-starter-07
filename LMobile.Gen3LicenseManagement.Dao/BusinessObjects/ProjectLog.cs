using System;
using System.Collections.Generic;
namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public class ProjectLog : StoredProjectLog {
		public virtual Customer Customer { get; set; }
		public virtual Project Project { get; set; }
		public virtual Installation Installation { get; set; }
	}
}
