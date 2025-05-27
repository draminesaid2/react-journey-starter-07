using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public class StoredDeveloper : StoredObject {
		public virtual string Name { get; set; }
		public virtual string HardwareKey { get; set; }
	}
}
