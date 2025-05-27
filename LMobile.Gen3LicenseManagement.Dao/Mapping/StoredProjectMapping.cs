using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gida;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class StoredProjectMapping : StoredObjectMapping {
		public readonly GidaColumnMapping CustomerID;
		public readonly GidaColumnMapping ProjectGuid;
		public readonly GidaColumnMapping ContractNo;
		public readonly GidaColumnMapping ProjectType;
		public readonly GidaColumnMapping Description;
		public readonly GidaColumnMapping EMail;
		public readonly GidaColumnMapping NotifyEMail;
		public readonly GidaColumnMapping ExpiryDate;
		public readonly GidaColumnMapping ExpiryInMonths;
		public readonly GidaColumnMapping ProductiveLicenseCount;
		public readonly GidaColumnMapping TestLicenseCount;
		public readonly GidaColumnMapping UserData;
		public readonly GidaColumnMapping XtendedStartDate;
		public readonly GidaColumnMapping XtendedFinishDate;
		public readonly GidaColumnMapping XtendedLicenseCount;
		public readonly GidaColumnMapping XtendedEachYear;
		public StoredProjectMapping(GidaSession session)
			: base(session, "Project")
		{
			var define = this.Define<StoredProject>();
			this.CustomerID = define.Column("CustomerID", o => o.CustomerID, (o, value) => o.CustomerID = value);
			this.ProjectGuid = define.Column("ProjectGuid", o => o.ProjectGuid, (o, value) => o.ProjectGuid = value);
			this.ContractNo = define.Column("ContractNo", 64, o => o.ContractNo, (o, value) => o.ContractNo = value);
			this.ProjectType = define.Column("ProjectType", 32, o => o.ProjectType, (o, value) => o.ProjectType = value);
			this.Description = define.Column("Description", 256, o => o.Description, (o, value) => o.Description = value);
			this.EMail = define.Column("EMail", 256, o => o.EMail, (o, value) => o.EMail = value);
			this.NotifyEMail = define.Column("NotifyEMail", 256, o => o.NotifyEMail, (o, value) => o.NotifyEMail = value);
			this.ExpiryDate = define.Column("ExpiryDate", o => o.ExpiryDate, (o, value) => o.ExpiryDate = value);
			this.ExpiryInMonths = define.Column("ExpiryInMonths", o => o.ExpiryInMonths, (o, value) => o.ExpiryInMonths = value);
			this.ProductiveLicenseCount = define.Column("ProductiveLicenseCount", o => o.ProductiveLicenseCount, (o, value) => o.ProductiveLicenseCount = value);
			this.TestLicenseCount = define.Column("TestLicenseCount", o => o.TestLicenseCount, (o, value) => o.TestLicenseCount = value);
			this.UserData = define.Column("UserData", 512, o => o.UserData, (o, value) => o.UserData = value);
			this.XtendedStartDate = define.Column("XtendedStartDate", o => o.XtendedStartDate, (o, value) => o.XtendedStartDate = value);
			this.XtendedFinishDate = define.Column("XtendedFinishDate", o => o.XtendedFinishDate, (o, value) => o.XtendedFinishDate = value);
			this.XtendedLicenseCount = define.Column("XtendedLicenseCount", o => o.XtendedLicenseCount, (o, value) => o.XtendedLicenseCount = value);
			this.XtendedEachYear = define.Column("XtendedEachYear", o => o.XtendedEachYear, (o, value) => o.XtendedEachYear = value);
		}
		protected override void DefineSchema(GidaTableMappingSchemaDefinition define) {
			base.DefineSchema(define);
			define.UniqueIndex("Project_I1", this.ContractNo);
			define.ForeignKey<StoredCustomerMapping>("Project_FK1", (f, add) => add.Column(this.CustomerID, f.ID));
			define.ForeignKey<StoredProjectTypeMapping>("Project_FK2", (f, add) => add.Column(this.ProjectType, f.ProjectType));
		}
		protected override void OnInserting(object entity) {
			base.OnInserting(entity);
			var project = (StoredProject)entity;
			project.ProjectGuid = Guid.NewGuid();
		}
	}
}
