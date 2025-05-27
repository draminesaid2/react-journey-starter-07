using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gida;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class StoredProjectModulePropertyMapping : StoredObjectMapping {
		public readonly GidaColumnMapping ProjectID;
		public readonly GidaColumnMapping ModulePropertyID;
		public readonly GidaColumnMapping ExpiryDate;
		public readonly GidaColumnMapping ExpiryInMonths;
		public readonly GidaColumnMapping ProductiveLicenseCount;
		public readonly GidaColumnMapping TestLicenseCount;
		public readonly GidaColumnMapping XtendedStartDate;
		public readonly GidaColumnMapping XtendedFinishDate;
		public readonly GidaColumnMapping XtendedLicenseCount;
		public readonly GidaColumnMapping XtendedEachYear;
		public StoredProjectModulePropertyMapping(GidaSession session)
			: base(session, "ProjectModuleProperty")
		{
			var define = this.Define<StoredProjectModuleProperty>();
			this.ProjectID = define.Column("ProjectID", o => o.ProjectID, (o, value) => o.ProjectID = value);
			this.ModulePropertyID = define.Column("ModulePropertyID", o => o.ModulePropertyID, (o, value) => o.ModulePropertyID = value);
			this.ExpiryDate = define.Column("ExpiryDate", o => o.ExpiryDate, (o, value) => o.ExpiryDate = value);
			this.ExpiryInMonths = define.Column("ExpiryInMonths", o => o.ExpiryInMonths, (o, value) => o.ExpiryInMonths = value);
			this.ProductiveLicenseCount = define.Column("ProductiveLicenseCount", o => o.ProductiveLicenseCount, (o, value) => o.ProductiveLicenseCount = value);
			this.TestLicenseCount = define.Column("TestLicenseCount", o => o.TestLicenseCount, (o, value) => o.TestLicenseCount = value);
			this.XtendedStartDate = define.Column("XtendedStartDate", o => o.XtendedStartDate, (o, value) => o.XtendedStartDate = value);
			this.XtendedFinishDate = define.Column("XtendedFinishDate", o => o.XtendedFinishDate, (o, value) => o.XtendedFinishDate = value);
			this.XtendedLicenseCount = define.Column("XtendedLicenseCount", o => o.XtendedLicenseCount, (o, value) => o.XtendedLicenseCount = value);
			this.XtendedEachYear = define.Column("XtendedEachYear", o => o.XtendedEachYear, (o, value) => o.XtendedEachYear = value);
		}
		protected override void DefineSchema(GidaTableMappingSchemaDefinition define) {
			base.DefineSchema(define);
			define.UniqueIndex("ProjectModuleProperty_I1", this.ProjectID, this.ModulePropertyID );
			define.ForeignKey<StoredProjectMapping>("ProjectModuleProperty_FK1", (f, add) => add.Column(this.ProjectID, f.ID));
			define.ForeignKey<StoredModulePropertyMapping>("ProjectModuleProperty_FK2", (f, add) => add.Column(this.ModulePropertyID, f.ID));
		}
	}
}
