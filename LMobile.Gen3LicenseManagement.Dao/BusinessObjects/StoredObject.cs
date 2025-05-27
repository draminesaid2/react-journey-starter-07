using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public class StoredObject {
		public virtual int ID { get; internal set; }
		public virtual string CreateUser { get; set; }
		public virtual DateTime CreateDate { get; set; }
		public virtual string ModifyUser { get; set; }
		public virtual DateTime? ModifyDate { get; set; }
		public virtual int UpdateCount { get; set; }
	}
}
