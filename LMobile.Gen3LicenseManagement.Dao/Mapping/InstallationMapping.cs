
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gida;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class InstallationMapping : GidaViewMapping {
		public readonly StoredInstallationMapping Main;
		public readonly ProjectMapping Project;
		public InstallationMapping(GidaSession session)
			: base(session) {
			var define = this.Define<Installation>();
			this.Main = this.From<StoredInstallationMapping>();
			define.SelectIntoResult(this.Main);
			this.Project = this.MergedInnerJoin<ProjectMapping>(c => c.Main.ID == this.Main.ProjectID);
			define.Select<Project>(this.Project, (o, value) => o.Project = value);
			}
	}
}
