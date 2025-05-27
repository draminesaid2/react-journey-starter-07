using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gida;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class StoredProjectLogMapping : StoredObjectMapping {
		public readonly GidaColumnMapping CustomerID;
		public readonly GidaColumnMapping ProjectID;
		public readonly GidaColumnMapping InstallationID;
		public readonly GidaColumnMapping EventTime;
		public readonly GidaColumnMapping UserName;
		public readonly GidaColumnMapping MessageType;
		public readonly GidaColumnMapping Message;
		public readonly GidaColumnMapping MessageLong;
		public readonly GidaColumnMapping Stacktrace;
		public StoredProjectLogMapping(GidaSession session)
			: base(session, "ProjectLog")
		{
			var define = this.Define<StoredProjectLog>();
			this.CustomerID = define.Column("CustomerID", o => o.CustomerID, (o, value) => o.CustomerID = value);
			this.ProjectID = define.Column("ProjectID", o => o.ProjectID, (o, value) => o.ProjectID = value);
			this.InstallationID = define.Column("InstallationID", o => o.InstallationID, (o, value) => o.InstallationID = value);
			this.EventTime = define.Column("EventTime", o => o.EventTime, (o, value) => o.EventTime = value);
			this.UserName = define.Column("UserName", 32, o => o.UserName, (o, value) => o.UserName = value);
			this.MessageType = define.Column("MessageType", 42, o => o.MessageType, (o, value) => o.MessageType = value);
			this.Message = define.Column("Message", 256, o => o.Message, (o, value) => o.Message = value);
			this.MessageLong = define.Column("MessageLong", o => o.MessageLong, (o, value) => o.MessageLong = value);
			this.Stacktrace = define.Column("Stacktrace", o => o.Stacktrace, (o, value) => o.Stacktrace = value);
		}
		protected override void DefineSchema(GidaTableMappingSchemaDefinition define) {
			base.DefineSchema(define);
			define.Index("ProjectLog_I1", this.CustomerID);
			define.Index("ProjectLog_I2", this.ProjectID);
			define.Index("ProjectLog_I3", this.InstallationID);
			define.Index("ProjectLog_I4", this.MessageType);
			define.Index("ProjectLog_I5", this.Message);
		}
		protected override void OnInserting(object entity) {
			base.OnInserting(entity);
			var projectLog = (StoredProjectLog)entity;
			
		}
	}
}
