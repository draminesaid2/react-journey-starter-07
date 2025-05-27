using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public class StoredCustomer : StoredObject {
		public virtual string CustomerNo { get; set; }
		public virtual string Name1 { get; set; }
		public virtual string Name2 { get; set; }
		public virtual string Address { get; set; }
		public virtual string City { get; set; }
		public virtual string DefaultEMail { get; set; }

	}
}
