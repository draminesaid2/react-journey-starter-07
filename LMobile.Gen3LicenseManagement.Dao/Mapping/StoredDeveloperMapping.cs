using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gida;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class StoredDeveloperMapping : StoredObjectMapping {
		public readonly GidaColumnMapping Name;
		public readonly GidaColumnMapping HardwareKey;
		public StoredDeveloperMapping(GidaSession session)
			: base(session, "Developer")
		{
			var define = this.Define<StoredDeveloper>();
			this.Name = define.Column("Name", 256, o => o.Name, (o, value) => o.Name = value);
			this.HardwareKey = define.Column("HardwareKey", 256, o => o.HardwareKey, (o, value) => o.HardwareKey = value);
		}
		protected override void DefineSchema(GidaTableMappingSchemaDefinition define) {
			base.DefineSchema(define);
			define.UniqueIndex("Developer_I1", this.HardwareKey);
		}
	}
}
