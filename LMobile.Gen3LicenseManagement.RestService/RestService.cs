using System;
using System.Text;
using System.Collections.Generic;
using LMobile.Gida;
using LMobile.WebServer;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gen3LicenseManagement.Dao.Contracts;
using LMobile.Gen3LicenseManagement.Dao.Services;
using System.Net.Mail;

namespace LMobile.Gen3LicenseManagement.RestService {
	class RestService : WebServerListener {
		public volatile Dictionary<string, DateTime> ExpiryEmails = new Dictionary<string, DateTime>();
		public RestService() {
#if DEBUG
			//this.AddPrefix("http://licenses.l-mobile.com/");
			this.AddPrefix("http://localhost/");
#else
			this.AddPrefix("http://licenses.l-mobile.com/");
#endif
			this.Realm = "L-mobile";
			this.MaxConcurrentRequests = 32;
			this.Routes.RegisterRoute(WebServerRoute.Create("Request")
				.SetHandler(WebMethod.Post, this.ProcessLicenseRequest));
			this.Routes.RegisterRoute(WebServerRoute.Create("CrashReport")
				.SetHandler(WebMethod.Post, this.ProcessCrashReport));
		}
		private void ProcessLicenseRequest(WebServerContext context) {
			try {
				if ((context.Request.ContentLength64 > 0) && (context.Request.ContentLength64 < 65536) && (context.Request.ContentType == "text/plain")) {
					var requestBytes = new byte[context.Request.ContentLength64];
					var bytesRead = context.Request.InputStream.Read(requestBytes, 0, requestBytes.Length);
					if (bytesRead == requestBytes.Length) {
						var request = Encoding.ASCII.GetString(requestBytes);
						var license = this.GetLicenseForRequest(request);
						//Console.WriteLine(request);
						if (license != null) {
							var responseBytes = Encoding.ASCII.GetBytes(license);
							context.SendResponse("text/plain", responseBytes);
							return;
						}
					}
				}
			} catch (Exception ex) {
				throw ex;
			}
			throw new WebServerBadRequestException();
		}
		private string GetLicenseForRequest(string request) {
			var session = GidaConfiguration.Factories["LicenseManagement"].CurrentSession;
			var licenseDao = session.GetService<ILicenseDao>();
			licenseDao.ImportLicenseRequest(request);
			var license = licenseDao.CreateLicenseForRequest(request);
			var requestData = new LicenseRequest(request);
			var project = licenseDao.GetProject(requestData.ContractNumber);

			if (license == null) {
			} else {
				if (project.ExpiryDate < (DateTime.Now.Date.AddDays(7))) {
					var sendEMail = false;
					if (!this.ExpiryEmails.ContainsKey(project.ContractNo)) {
						this.ExpiryEmails[project.ContractNo] = DateTime.Now.Date;
						sendEMail = true;
					} else if(DateTime.Now.Date.Subtract(this.ExpiryEmails[project.ContractNo]).TotalDays < 1) {
						sendEMail = false;
					} else {
						this.ExpiryEmails[project.ContractNo] = DateTime.Now.Date;
						sendEMail = true;
					}

					if (sendEMail) {
						var smtpClient = NetworkConfiguration.CreateSmtpClient();
						var message = "Project License for " + project.Customer.Name1 + " expires in " + Math.Floor(project.ExpiryDate.Subtract(DateTime.Now.Date).TotalDays).ToString() + " days.";
						using (MailMessage mailMessage = new MailMessage()) {
							mailMessage.From = new MailAddress(NetworkConfiguration.MailFrom);
							var eMailRecipients = project.NotifyEMail;
							foreach (var r in eMailRecipients.Split(',')) {
								if (!string.IsNullOrEmpty(r) && r.ToUpper().EndsWith("@L-MOBILE.COM"))
									mailMessage.To.Add(r);
							}
							mailMessage.Subject = "License-Management Expiry-Date reached for Project " + project.Customer.Name1;
							mailMessage.Body = "The License-Management produced the following message:\n" +
								message + "\n";
							mailMessage.Body += "linked to Customer '" + project.Customer.Name1 + "' (" + project.Customer.ID + ")\n";
							mailMessage.Body += "linked to Project '" + project.Description + "' (" + project.ContractNo + ")";
							try {
								smtpClient.Send(mailMessage);
								licenseDao.LogEntry(project.Customer.ID, 
									project.ID, 
									null, 
									MessageTypes.InstallationAutoAcceptedActivated,
									project.Description + " License expired", "License-Management Expiry-Date reached \nfor Project " + project.Description + " \nat Customer " + project.Customer.Name1 + " \n eMail to " + project.NotifyEMail);
							} catch { }
						}
					}
				}
			}
			return license;
		}
		private void ProcessCrashReport(WebServerContext context) {
			try {
				if ((context.Request.ContentLength64 > 0) && (context.Request.ContentLength64 < 1024) && (context.Request.ContentType == "text/plain")) {
					var requestBytes = new byte[context.Request.ContentLength64];
					var bytesRead = context.Request.InputStream.Read(requestBytes, 0, requestBytes.Length);
					if (bytesRead == requestBytes.Length) {
						// TODO : Insert Crash Report
					}
				}
			} catch { }
			throw new WebServerBadRequestException();
		}
	}
}
