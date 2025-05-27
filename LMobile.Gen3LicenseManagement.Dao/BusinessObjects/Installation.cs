using System;
namespace LMobile.Gen3LicenseManagement.Dao.BusinessObjects {
	public class Installation : StoredInstallation {
		public virtual Project Project { get; set; }
		private LicenseRequest myLicensedRequestData;
		public virtual LicenseRequest LicensedRequestData {
			get {
				if (string.IsNullOrEmpty(this.LicensedRequest)) return null;
				if (myLicensedRequestData == null) myLicensedRequestData = new LicenseRequest(this.LicensedRequest);
				return myLicensedRequestData;
			}
		}
		private LicenseRequest myPendingRequestData;
		public virtual LicenseRequest PendingRequestData {
			get {
				if (string.IsNullOrEmpty(this.PendingRequest)) return null;
				if (myPendingRequestData == null) myPendingRequestData = new LicenseRequest(this.PendingRequest);
				return myPendingRequestData;
			}
		}
	}
}
