using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public class StoredProject : StoredObject {
		public virtual int CustomerID { get; set; }
		public virtual Guid ProjectGuid { get; internal set; }
		public virtual string ContractNo { get; set; }
		public virtual string ProjectType { get; set; }
		public virtual string Description { get; set; }
		public virtual string EMail { get; set; }
		public virtual string NotifyEMail { get; set; }
		public virtual DateTime ExpiryDate { get; set; }
		public virtual int? ExpiryInMonths { get; set; }
		public virtual int ProductiveLicenseCount { get; set; }
		public virtual int TestLicenseCount { get; set; }
		public virtual string UserData { get; set; }
		public virtual DateTime XtendedStartDate { get; set; }
		public virtual DateTime XtendedFinishDate { get; set; }
		public virtual int XtendedLicenseCount { get; set; }
		public virtual bool XtendedEachYear { get; set; }
		//public virtual int? AddTempLicenseCount { get; set; }
		//public virtual DateTime? AddTempLicenseExpirationDate { get; set; }
	}
}
