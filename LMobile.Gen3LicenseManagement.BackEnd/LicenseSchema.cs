using System;
using System.Collections.Generic;
using System.Text;

namespace LMobile.Gen3LicenseManagement {
	public class LicenseSchema : SerializationSchema {
		public static readonly LicenseSchemaVersion V0 = new LicenseSchemaVersion(0, "3.1.0");
		public static readonly LicenseSchemaVersion CurrentVersion = V0;

		public static readonly IList<LicenseSchemaVersion> Versions = new LicenseSchemaVersion[1] { V0 };

		public static readonly LicenseSchema Instance = new LicenseSchema();

		public LicenseSchema()
			: base(0x11CE45E, CurrentVersion.SchemaVersion) {
			LicenseData.Register(this);
			LicenseModule.Register(this);
		}
	}

	public class LicenseSchemaVersion {
		private readonly string myAssemblyVersion;
		private readonly int mySchemaVersion;
		internal LicenseSchemaVersion(int schemaVersion, string assemblyVersion) {
			mySchemaVersion = schemaVersion;
			myAssemblyVersion = assemblyVersion;
		}
		public string AssemblyVersion { get { return myAssemblyVersion; } }
		public int SchemaVersion { get { return mySchemaVersion; } }
	}

}
