using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public class StoredModulePropertiesInModules : StoredObject {
		public virtual int ModuleID { get; set; }
		public virtual int ModulePropertyID { get; set; }
	}
}
