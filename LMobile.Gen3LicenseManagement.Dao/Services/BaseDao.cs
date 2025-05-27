using LMobile.Gida;

namespace LMobile.Gen3LicenseManagement.Dao.Services {
	public abstract class BaseDao  {
		private readonly GidaSession mySession;
		protected BaseDao(GidaSession session) {
			mySession = session;
		}
		public GidaSession Session {
			get { return mySession; }
		}
	}
}
