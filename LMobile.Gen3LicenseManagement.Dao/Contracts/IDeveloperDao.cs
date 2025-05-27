using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;

namespace LMobile.Gen3LicenseManagement.Dao.Contracts {
	public interface IDeveloperDao {
		List<StoredDeveloper> GetDevelopers();
		List<StoredDeveloper> GetDevelopers(string searchKey);
		StoredDeveloper GetDeveloper(int developerID);
		StoredDeveloper GetDeveloper(string hardwareKey);
	}
}
