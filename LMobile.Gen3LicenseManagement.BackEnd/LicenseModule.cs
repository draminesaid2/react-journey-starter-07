using System;
using System.Collections.Generic;
using System.Text;

namespace LMobile.Gen3LicenseManagement {
	public sealed class LicenseModule : Gen3LicenseModule {
		private string myModuleID;
		private int? myLicenseCount;
		private string myUserData;
		public LicenseModule() { }
		public string ModuleID {
			get { return myModuleID; }
			set { myModuleID = value; }
		}
		public int? LicenseCount {
			get { return myLicenseCount; }
			set { myLicenseCount = value; }
		}
		public string UserData {
			get { return myUserData; }
			set { myUserData = value; }
		}
		internal static void Register(SerializationSchema schema) {
			schema.RegisterEntity<LicenseModule>("module", 
				(context, entity) => entity.Serialize(context));
		}
		internal void Serialize(SerializationContext serialize) {
			serialize.Value("ModuleID", ref myModuleID);
			serialize.OptionalValue("LicenseCount", ref myLicenseCount);
			serialize.OptionalValue("UserData", ref myUserData);
		}
		public override bool Equals(object obj) {
			var other = obj as Gen3LicenseModule;
			if (other == null) return false;
			return this.ModuleID == other.ModuleID
				&& this.LicenseCount == other.LicenseCount
				&& this.UserData == other.UserData;
		}
		public override int GetHashCode() {
			return (this.ModuleID ?? String.Empty).GetHashCode()
				^ this.LicenseCount.GetHashCode()
				^ (this.UserData ?? String.Empty).GetHashCode();
		}
	}
}
