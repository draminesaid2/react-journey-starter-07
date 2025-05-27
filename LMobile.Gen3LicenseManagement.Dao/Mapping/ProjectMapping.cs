using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gida;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class ProjectMapping : GidaViewMapping {
		public readonly StoredProjectMapping Main;
		public readonly CustomerMapping Customer;
		public readonly GidaColumnMapping InstallationCount;
		public ProjectMapping(GidaSession session)
			: base(session) {
			var define = this.Define<Project>();
			this.Main = this.From<StoredProjectMapping>();
			define.SelectIntoResult(this.Main);
			this.Customer = this.MergedInnerJoin<CustomerMapping>(c => c.Main.ID == this.Main.CustomerID);
			define.Select<Customer>(this.Customer, (o, value) => o.Customer = value);
			this.InstallationCount = define.Select(
				this.Session.Query().From<StoredInstallationMapping>().Where(p => p.ProjectID == this.Main.ID).Select(p => Gql.Count()),
				(o, value) => o.InstallationCount = value);
		}
	}
}
