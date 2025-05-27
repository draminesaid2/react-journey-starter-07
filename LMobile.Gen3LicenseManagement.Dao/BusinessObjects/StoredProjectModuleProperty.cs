using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public class StoredProjectModuleProperty : StoredObject {
		public virtual int ProjectID { get; set; }
		public virtual int ModulePropertyID { get; set; }
		public virtual DateTime ExpiryDate { get; set; }
		public virtual int? ExpiryInMonths { get; set; }
		public virtual int ProductiveLicenseCount { get; set; }
		public virtual int TestLicenseCount { get; set; }
		public virtual DateTime XtendedStartDate { get; set; }
		public virtual DateTime XtendedFinishDate { get; set; }
		public virtual int XtendedLicenseCount { get; set; }
		public virtual bool XtendedEachYear { get; set; }
	}
}
