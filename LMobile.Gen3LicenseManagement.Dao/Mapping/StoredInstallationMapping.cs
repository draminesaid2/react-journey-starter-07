using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gida;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class StoredInstallationMapping : StoredObjectMapping
	{
		public readonly GidaColumnMapping ProjectID;
		public readonly GidaColumnMapping InstallationType;
		public readonly GidaColumnMapping AutoAcceptRequests;
		public readonly GidaColumnMapping LicensedRequest;
		public readonly GidaColumnMapping PendingRequest;
        public readonly GidaColumnMapping Portable;
		public readonly GidaColumnMapping Version;

		public StoredInstallationMapping(GidaSession session)
			: base(session, "Installation")
		{
			var define = this.Define<StoredInstallation>();
			this.ProjectID = define.Column("ProjectID", o => o.ProjectID, (o, value) => o.ProjectID = value);
			this.InstallationType = define.Column("InstallationType", 32, o => o.InstallationType, (o, value) => o.InstallationType = value);
			this.AutoAcceptRequests = define.Column("ActivatedForDownload", o => o.AutoAcceptRequests, (o, value) => o.AutoAcceptRequests = value);
			this.LicensedRequest = define.Column("LicensedRequest", o => o.LicensedRequest, (o, value) => o.LicensedRequest = value);
			this.PendingRequest = define.Column("PendingRequest", o => o.PendingRequest, (o, value) => o.PendingRequest = value);
            this.Portable = define.Column("Portable", o => o.Portable, (o, value) => o.Portable = value);
			this.Version = define.Column("Version", o => o.Version, (o, value) => o.Version = value);
		}
		protected override void DefineSchema(GidaTableMappingSchemaDefinition define) {
			base.DefineSchema(define);
			define.ForeignKey<StoredProjectMapping>("Installation_FK1", (f, add) => add.Column(this.ProjectID, f.ID));
			define.UniqueIndex("Installation_I1", this.ProjectID, this.InstallationType);
		}
	}
}
