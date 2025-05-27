using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gen3LicenseManagement.Dao.Contracts;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gen3LicenseManagement.Dao.Mapping;
using LMobile.Gida;

namespace LMobile.Gen3LicenseManagement.Dao.Services {
	internal class DeveloperDao : BaseDao, IDeveloperDao {
		public DeveloperDao(GidaSession session)
			: base(session) {
		}

		public List<StoredDeveloper> GetDevelopers() {
			return GetDevelopers(null);
		}

		public List<StoredDeveloper> GetDevelopers(string searchKey) {
			var query = Session.Query<StoredDeveloper, StoredDeveloperMapping>().OrderBy(x => x.Name, x => x.HardwareKey);
			if (!String.IsNullOrEmpty(searchKey)) {
				searchKey = String.Concat("%", searchKey.ToUpper(), "%");
				query.Where(l => l.Name.ToUpper().Like(searchKey) |
					l.HardwareKey.ToUpper().Like(searchKey));
			}
			return query.ReadList();
		}

		public StoredDeveloper GetDeveloper(int developerID) {
			if (developerID == 0) return null;
			var query = Session.Query<StoredDeveloper, StoredDeveloperMapping>();
			query.Where(s => s.ID == developerID);
			return query.ReadFirst();
		}

		public StoredDeveloper GetDeveloper(string hardwareKey) {
			if (String.IsNullOrEmpty(hardwareKey)) return null;
			var query = Session.Query<StoredDeveloper, StoredDeveloperMapping>();
			query.Where(s => s.HardwareKey == hardwareKey);
			return query.ReadFirst();
		}
	}
}
