using System;
using System.Collections.Generic;
using System.Text;

namespace LMobile.Gen3LicenseManagement.RestService {
	class Program {
		static void Main(string[] args) {
			new Service().Run(args);
		}
	}
	class Service : ServiceProcess {
		private RestService myRestService;
		protected override void OnStart(bool isService, string[] args) {
			myRestService = new RestService();
			myRestService.Start();
		}
		protected override void OnStop() {
			if (myRestService != null) myRestService.Stop();
		}
	}
}
