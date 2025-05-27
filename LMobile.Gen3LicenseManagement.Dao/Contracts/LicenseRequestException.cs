using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMobile.Gen3LicenseManagement.Dao.Contracts {
	public class LicenseRequestException : Exception {
		private readonly LicenseRequestResults myResult;
		private readonly string myProjectType;
		public LicenseRequestException(LicenseRequestResults result)
			: base(result.ToString()) {
			myResult = result;
		}
		public LicenseRequestException(LicenseRequestResults result, Exception innerException)
			: base(result.ToString(), innerException) {
			myResult = result;
		}
		public LicenseRequestException(LicenseRequestResults result, string projectType)
			: base(result.ToString()) {
			myResult = result;
			myProjectType = projectType;
		}
		public LicenseRequestResults Result {
			get { return myResult; }
		}
		public string ProjectType {
			get { return myProjectType; }
		}
	}
}
