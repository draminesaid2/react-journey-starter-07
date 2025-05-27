using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gida;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class StoredCustomerMapping : StoredObjectMapping {
		public readonly GidaColumnMapping CustomerNo;
		public readonly GidaColumnMapping Name1;
		public readonly GidaColumnMapping Name2;
		public readonly GidaColumnMapping Address;
		public readonly GidaColumnMapping City;
		public readonly GidaColumnMapping DefaultEMail;
		public StoredCustomerMapping(GidaSession session)
			: base(session, "Customer")
		{
			var define = this.Define<StoredCustomer>();
			this.CustomerNo = define.Column("CustomerNo", 64, o => o.CustomerNo, (o, value) => o.CustomerNo = value);
			this.Name1 = define.Column("Name1", 256, o => o.Name1, (o, value) => o.Name1 = value);
			this.Name2 = define.Column("Name2", 256, o => o.Name2, (o, value) => o.Name2 = value);
			this.Address = define.Column("Address", 256, o => o.Address, (o, value) => o.Address = value);
			this.City = define.Column("City", 256, o => o.City, (o, value) => o.City = value);
			this.DefaultEMail = define.Column("DefaultEMail", 256, o => o.DefaultEMail, (o, value) => o.DefaultEMail = value);				
		}
		protected override void DefineSchema(GidaTableMappingSchemaDefinition define) {
			base.DefineSchema(define);
			define.UniqueIndex("Customer_I1", this.CustomerNo);
		}
	}
}
