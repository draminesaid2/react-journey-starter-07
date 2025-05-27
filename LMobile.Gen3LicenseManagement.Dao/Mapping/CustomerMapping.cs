using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gida;

namespace LMobile.Gen3LicenseManagement.Dao.Mapping {
	public class CustomerMapping : GidaViewMapping
	{
		public readonly StoredCustomerMapping Main;
		public readonly GidaColumnMapping ProjectCount;
		public CustomerMapping(GidaSession session)
			: base(session) {
			var define = this.Define<Customer>();
			this.Main = this.From<StoredCustomerMapping>();
			define.SelectIntoResult(this.Main);
			this.ProjectCount = define.Select(
				this.Session.Query().From<StoredProjectMapping>().Where(p => p.CustomerID == this.Main.ID).Select(p => Gql.Count()), 
				(o, value) => o.ProjectCount = value);
		}
	}
}
