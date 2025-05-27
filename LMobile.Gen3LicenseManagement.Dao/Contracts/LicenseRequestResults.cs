using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMobile.Gen3LicenseManagement.Dao.Contracts {
	public enum LicenseRequestResults {
		RequestCantBeParsed,
		ProjectNotFound,
		ProjectGuidNotValid,
		ProjectTypesDontMatch,
		InstallationTypeNotFound,
		NotPortableExists,
	}
}
