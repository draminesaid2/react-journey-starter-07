using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMobile.Gida;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class ProjectLogMapping : GidaViewMapping {
		public readonly StoredProjectLogMapping Main;
		public readonly CustomerMapping Customer;
		public readonly ProjectMapping Project;
		public readonly InstallationMapping Installation;
		public ProjectLogMapping(GidaSession session)
			: base(session) {
			var define = this.Define<ProjectLog>();
			this.Main = this.From<StoredProjectLogMapping>();
			define.SelectIntoResult(this.Main);
			this.Customer = this.MergedInnerJoin<CustomerMapping>(c => c.Main.ID == this.Main.CustomerID);
			define.Select<Customer>(this.Customer, (o, value) => o.Customer = value);
			this.Project = this.MergedLeftOuterJoin<ProjectMapping>(c => c.Main.ID == this.Main.ProjectID);
			define.Select<Project>(this.Project, (o, value) => o.Project = value);
			this.Installation = this.MergedLeftOuterJoin<InstallationMapping>(c => c.Main.ID == this.Main.InstallationID);
			define.Select<Installation>(this.Installation, (o, value) => o.Installation = value);
		}
	}
}
