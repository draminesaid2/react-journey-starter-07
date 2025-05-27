using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public enum MessageTypes {
		CustomerCreated,
		CustomerModified,
		ProjectCreated,
		ProjectModified,
		InstallationImported,
		InstallationCreated,
		InstallationAutoAccepted,
		InstallationActivated,
		InstallationExported,
		InstallationCopied,
		InstallationAutoAcceptedActivated,
		HardwareKeyChanged,
		InstallationRequested,
		InvalidInstallationRequest,
		ModuleCreated,
		ModuleModified,
		ModulePropertyCreated,
		ModulePropertyModified,
		ModuleAdded,
		InstallationDeleted
	}
	public class StoredProjectLog : StoredObject {
		public virtual int? CustomerID { get; set; }
		public virtual int? ProjectID { get; set; }
		public virtual int? InstallationID { get; set; }
		public virtual DateTime EventTime { get; set; }
		public virtual string UserName { get; set; }
		public virtual string MessageType { get; set; }
		public virtual string Message { get; set; }
		public virtual string MessageLong { get; set; }
		public virtual string Stacktrace { get; set; }
	}
}
