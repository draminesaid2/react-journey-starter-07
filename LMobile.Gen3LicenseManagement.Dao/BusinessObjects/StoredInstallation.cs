using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public class StoredInstallation : StoredObject {
		public virtual int ProjectID { get; set; }
		public virtual string InstallationType { get; set; }
		public virtual string LicensedRequest { get; set; }
		public virtual string PendingRequest { get; set; }
		public virtual bool AutoAcceptRequests { get; set; }
        public virtual bool Portable { get; set; }
		public virtual int Version { get; set; }
	}
}
