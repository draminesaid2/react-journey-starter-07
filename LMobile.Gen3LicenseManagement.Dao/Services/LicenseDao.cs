using System.Collections.Generic;
using LMobile.Gida;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;
using LMobile.Gen3LicenseManagement.Dao.Contracts;
using LMobile.Gen3LicenseManagement.Dao.Mapping;
using System;
using System.Linq;
using System.Threading;
using System.Net.Mail;

namespace LMobile.Gen3LicenseManagement.Dao.Services {
	internal class LicenseDao : BaseDao, ILicenseDao {
		public LicenseDao(GidaSession session)
			: base(session) {
		}

		//MLI 2017-11-29: Search by ContractNo
		/// <summary>
		/// Get customer list by project/contract
		/// </summary>
		/// <param name="p_ContractNo"></param>
		/// <returns></returns>
		public List<Customer> GetCustomersByContractNo(string p_ContractNo) {
			List<Customer> result = null;
			if (!string.IsNullOrEmpty(p_ContractNo)) {
				string contractNo = string.Concat("%", p_ContractNo.ToUpper(), "%");

				GidaFluentQuery<Customer, CustomerMapping> query
					= Session.Query<Customer, CustomerMapping>()
						.Where(x => Gql.Exists(Session.Query()
																	.From<ProjectMapping>()
																	.Where(prj => prj.Main.ContractNo.Like(contractNo)
																	& prj.Main.CustomerID == x.Main.ID))
									)
							 .OrderBy(s => s.Main.Name1);

				result = query.ReadList();
			} else {
				result = new List<Customer>();
			}
			return (result);
		}
		public List<Customer> GetCustomersByPropertyName(string p_PropertyName) {
			List<Customer> result = null;
			if (!string.IsNullOrEmpty(p_PropertyName)) {
				string propertyName = p_PropertyName;

				GidaFluentQuery<Customer, CustomerMapping> query
					= Session.Query<Customer, CustomerMapping>()
						.Where(x => Gql.Exists(Session.Query()
																	.From<ProjectMapping>()
																	.InnerJoin<ProjectModulePropertyMapping>((p, pmp) => pmp.Main.ProjectID == p.Main.ID)
																	.InnerJoin<ModulePropertyMapping>((p, pmp, mp) => mp.Main.ID == pmp.Main.ModulePropertyID)
																	.Where((p, pmp, mp) => p.Main.CustomerID == x.Main.ID)
																	.Where((p, pmp, mp) => mp.Main.PropertyName.ToUpper() == propertyName.ToUpper())
																	)
									)
							.OrderBy(s => s.Main.Name1);

				result = query.ReadList();
			} else {
				result = new List<Customer>();
			}
			/*
					select c.* 
					from Customer c 
					inner join Project p on p.CustomerID = c.ID 
					inner join ProjectModuleProperty pmp on pmp.ProjectID = p.ID
					inner join ModuleProperty mp on mp.ID = pmp.ModulePropertyID 
					where mp.PropertyName = 'WH3004'
			*/
			return (result);
		}
		// Customers
		public List<Customer> GetCustomers(string searchKey) {
			var query = Session.Query<Customer, CustomerMapping>().OrderBy(s => s.Main.Name1);
			if (!string.IsNullOrEmpty(searchKey)) {
				searchKey = string.Concat("%", searchKey.ToUpper(), "%");
				query.Where(l => l.Main.CustomerNo.ToUpper().Like(searchKey)
					| l.Main.Name1.ToUpper().Like(searchKey)
					| l.Main.Name2.ToUpper().Like(searchKey));
			}
			return query.ReadList();
		}
		public Customer GetCustomer(int id) {
			var query = Session.Query<Customer, CustomerMapping>();
			query.Where(l => l.Main.ID == id);
			return query.ReadFirst();
		}
		public Customer GetCustomer(string customerNo) {
			if (string.IsNullOrEmpty(customerNo)) return null;
			var query = Session.Query<Customer, CustomerMapping>();
			query.Where(l => l.Main.CustomerNo.ToUpper() == customerNo);
			return query.ReadFirst();
		}

		// Projects
		public List<Project> GetProjectsByCustomer(int customerID) {
			return this.Session.Query<Project, ProjectMapping>()
								 .Where(x => x.Main.CustomerID == customerID)
								 .OrderBy(x => x.Main.ContractNo)
								 .ReadList();
		}
		public Project GetProject(int id) {
			var query = Session.Query<Project, ProjectMapping>();
			query.Where(l => l.Main.ID == id);
			return query.ReadFirst();
		}
		public Project GetProject(string contractNo) {
			if (string.IsNullOrEmpty(contractNo)) return null;
			var query = Session.Query<Project, ProjectMapping>();
			query.Where(l => l.Main.ContractNo.ToUpper() == contractNo);
			return query.ReadFirst();
		}
		public Project GetProject(string contractNo, Guid projectGuid) {
			if (string.IsNullOrEmpty(contractNo)) return null;
			var query = Session.Query<Project, ProjectMapping>();
			query.Where(l => l.Main.ContractNo.ToUpper() == contractNo & l.Main.ProjectGuid == projectGuid);
			return query.ReadFirst();
		}
		public List<ProjectModuleProperty> GetProjectModuleProperties(int projectID) {
			if (projectID == 0) return null;
			var query = Session.Query<ProjectModuleProperty, ProjectModulePropertyMapping>();
			query.Where(l => l.Main.ProjectID.ToUpper() == projectID);
			query.OrderBy(l => l.ModuleProperty.Main.PropertyName);
			return query.ReadList();
		}
		public ProjectModuleProperty GetProjectModuleProperty(int projectModulePropertyID) {
			if (projectModulePropertyID == 0) return null;
			var query = Session.Query<ProjectModuleProperty, ProjectModulePropertyMapping>();
			query.Where(l => l.Main.ID == projectModulePropertyID);
			return query.ReadFirst();
		}

		// Installations
		public List<Installation> GetByKeyword(string keyWord) {
			var query = Session.Query<Installation, InstallationMapping>().OrderBy(s => s.Project.Customer.Main.Name1);

			//if (generation != 0) query.Where(l => l.Generation == generation);
			if (!string.IsNullOrEmpty(keyWord)) {
				keyWord = string.Concat("%", keyWord.ToUpper(), "%");
				query.Where(l => l.Project.Main.ContractNo.ToUpper().Like(keyWord)
					| l.Project.Customer.Main.Name1.ToUpper().Like(keyWord)
					| l.Project.Customer.Main.Name2.ToUpper().Like(keyWord));
			}
			return query.ReadList();
		}
		public Installation GetInstallation(int ID) {
			var query = Session.Query<Installation, InstallationMapping>()
				.Where(l => l.Main.ID == ID);
			return query.ReadFirst();
		}
		public List<Installation> GetInstallationsByCustomer(int customerID) {
			return this.Session.Query<Installation, InstallationMapping>()
								 .Where(x => x.Project.Main.CustomerID == customerID)
								 .OrderBy(x => x.Project.Main.ContractNo, x => x.Main.InstallationType)
								 .ReadList();
		}
		public List<Installation> GetInstallationsByProject(int projectID) {
			return this.Session.Query<Installation, InstallationMapping>()
								 .Where(x => x.Main.ProjectID == projectID)
								 .OrderBy(x => x.Main.InstallationType)
								 .ReadList();
		}

		// Masterdata
		public List<StoredProjectType> GetProjectTypes() {
			var query = Session.Query<StoredProjectType, StoredProjectTypeMapping>();
			query.OrderBy(pt => pt.ProjectType);
			return query.ReadList();
		}
		public List<StoredInstallationType> GetInstallationTypes() {
			var query = Session.Query<StoredInstallationType, StoredInstallationTypeMapping>();
			query.OrderBy(it => it.InstallationType);
			return query.ReadList();
		}

		// Licenses
		public Installation ImportLicenseRequest(string request) {
			LicenseRequest dummy = null;
			bool isDeveloper = false;
			return this.ImportLicenseRequest(request, out dummy, out isDeveloper);
		}
		public Installation ImportLicenseRequest(string request, out LicenseRequest licenseRequest) {
			bool isDeveloper;
			return this.ImportLicenseRequest(request, out licenseRequest, out isDeveloper);
		}
		public Installation ImportLicenseRequest(string request, out LicenseRequest licenseRequest, out bool isDeveloper) {
			try {
				licenseRequest = new LicenseRequest(request);
			} catch (Exception exception) {
				throw new LicenseRequestException(LicenseRequestResults.RequestCantBeParsed, exception);
			}
			var project = this.GetProject(licenseRequest.ContractNumber);
			if (project == null) {
				LogEntry(null, null, MessageTypes.InvalidInstallationRequest, "Conntract-Number from request '" + licenseRequest.ContractNumber + "' doesn´t exist.");
				throw new LicenseRequestException(LicenseRequestResults.ProjectNotFound);
			}
			if (project.ProjectGuid != licenseRequest.ProjectGuid) {
				LogEntry(project.Customer.ID, project.ID, MessageTypes.InvalidInstallationRequest, "Project-GUID from request '" + licenseRequest.ProjectGuid + "' doesn´t match Project-GUID from project '" + project.ProjectGuid + "'.");
				throw new LicenseRequestException(LicenseRequestResults.ProjectGuidNotValid);
			}
			var installationType = licenseRequest.InstallationType;
			var installationTypes = this.GetInstallationTypes();
			if (!installationTypes.Any(it => it.InstallationType == installationType)) {
				LogEntry(project.Customer.ID, project.ID, MessageTypes.InvalidInstallationRequest, "Installation-Type from request '" + licenseRequest.InstallationType + "' doesn´t exist.");
				throw new LicenseRequestException(LicenseRequestResults.InstallationTypeNotFound);
			}

			var installations = this.GetInstallationsByProject(project.ID);
			var contractNumber = licenseRequest.ContractNumber;
			var installation = installations.FirstOrDefault(i =>
				i.InstallationType == installationType &&
				!i.Portable &&
				i.Project.ProjectType == project.ProjectType &&
				i.Project.ContractNo.ToUpper() == contractNumber.ToUpper());

			var developer = this.Session.GetService<IDeveloperDao>().GetDeveloper(licenseRequest.HardwareKey);
			isDeveloper = developer != null;

			if (!isDeveloper) {
				if (installation == null) {
					installation = new Installation {
						AutoAcceptRequests = false,
						InstallationType = installationType,
						LicensedRequest = null,
						PendingRequest = request,
						Portable = false,
						Project = project,
						ProjectID = project.ID,
						UpdateCount = 0,
					};
					Session.Insert(installation);
					LogEntry(project.Customer.ID, project.ID, installation.ID, MessageTypes.InstallationCreated, "New Installation created.", "Type is '" + installation.InstallationType + "', location is '" + licenseRequest.InstallationLocation + "' and hardware-Key is '" + licenseRequest.HardwareKey + "'.");
				} else if (installation.LicensedRequestData != null && (installation.LicensedRequestData.InstallationLocation != licenseRequest.InstallationLocation ||
					installation.LicensedRequestData.HardwareKey != licenseRequest.HardwareKey)) {
					installation.Portable = false;
					if (installation.AutoAcceptRequests) {
						installation.LicensedRequest = request;
						installation.PendingRequest = null;
						Session.Update(installation);
						LogEntry(project.Customer.ID, project.ID, installation.ID, MessageTypes.InstallationAutoAccepted, "Existing Installation automatically accepted.", "Type is '" + installation.InstallationType + "', location is '" + licenseRequest.InstallationLocation + "' and hardware-Key is '" + licenseRequest.HardwareKey + "'.");
					} else if (installation.PendingRequest != request) {
						installation.PendingRequest = request;
						Session.Update(installation);
						LogEntry(project.Customer.ID, project.ID, installation.ID, MessageTypes.InstallationRequested, "Existing Installation request pending.", "Type is '" + installation.InstallationType + "', location is '" + licenseRequest.InstallationLocation + "' and hardware-Key is '" + licenseRequest.HardwareKey + "'.");
					}
				} else if (installation.LicensedRequestData == null) {
					installation.Portable = false;
					if (installation.AutoAcceptRequests) {
						installation.LicensedRequest = request;
						installation.PendingRequest = null;
						Session.Update(installation);
						LogEntry(project.Customer.ID, project.ID, installation.ID, MessageTypes.InstallationAutoAccepted, "Existing Installation automatically accepted.", "Type is '" + installation.InstallationType + "', location is '" + licenseRequest.InstallationLocation + "' and hardware-Key is '" + licenseRequest.HardwareKey + "'.");
					} else if (installation.PendingRequest != request) {
						installation.PendingRequest = request;
						Session.Update(installation);
						LogEntry(project.Customer.ID, project.ID, installation.ID, MessageTypes.InstallationRequested, "Existing Installation request pending.", "Type is '" + installation.InstallationType + "', location is '" + licenseRequest.InstallationLocation + "' and hardware-Key is '" + licenseRequest.HardwareKey + "'.");
					}
				}
			}
			return installation;
		}

		public string CreateLicenseForRequest(string request) {
			LicenseRequest licenseRequest;
			try {
				licenseRequest = new LicenseRequest(request);
			} catch {
				return null;
			}

			var project = GetProject(licenseRequest.ContractNumber, licenseRequest.ProjectGuid);
			if (project == null) return null;

			var developer = this.Session.GetService<IDeveloperDao>().GetDeveloper(licenseRequest.HardwareKey);
			if (developer == null) {
				var installation = this.Session.Query<Installation, InstallationMapping>()
					.Where(x => x.Project.Main.ContractNo == licenseRequest.ContractNumber
						& x.Project.Main.ProjectGuid == licenseRequest.ProjectGuid
						& x.Main.InstallationType == licenseRequest.InstallationType
						& x.Main.Portable == false)
					.ReadFirst();
				if (installation == null) return null;
				if (installation.LicensedRequestData == null || installation.LicensedRequestData.InstallationLocation != licenseRequest.InstallationLocation ||
					installation.LicensedRequestData.HardwareKey != licenseRequest.HardwareKey) return null;
			}
			var licenseData = CreateLicenseData(project, licenseRequest.InstallationType, developer != null);
			return licenseData.Encrypt(licenseRequest);
		}

		public Installation CreatePortableInstallation(int projectID, string installationType, int version) {
			var project = this.GetProject(projectID);
			if (project == null) {
				throw new LicenseRequestException(LicenseRequestResults.ProjectNotFound);
			}
			var installationTypes = this.GetInstallationTypes();
			if (!installationTypes.Any(it => it.InstallationType == installationType)) {
				LogEntry(project.Customer.ID, project.ID, MessageTypes.InvalidInstallationRequest, "Installation-Type from request '" + installationType + "' doesn´t exist.");
				throw new LicenseRequestException(LicenseRequestResults.InstallationTypeNotFound);
			}

			var installations = this.GetInstallationsByProject(project.ID);
			var installation = installations.FirstOrDefault(i =>
				i.InstallationType == installationType);

			if (installation != null && !installation.Portable)
				throw new LicenseRequestException(LicenseRequestResults.NotPortableExists);

			if (installation == null) {
				installation = new Installation {
					AutoAcceptRequests = false,
					InstallationType = installationType,
					LicensedRequest = null,
					PendingRequest = null,
					Portable = true,
					Project = project,
					ProjectID = project.ID,
					Version = version,
					UpdateCount = 0,
				};
				Session.Insert(installation);
				LogEntry(project.Customer.ID, project.ID, installation.ID, MessageTypes.InstallationCreated, "New portable Installation created.", "Type is '" + installation.InstallationType + "', version is '" + installation.Version + "'.");
			} else if (installation.Version != version) {
				installation.Version = version;
				Session.Update(installation);
				LogEntry(project.Customer.ID, project.ID, installation.ID, MessageTypes.InstallationAutoAccepted, "Existing portable Installation updated.", "Type is '" + installation.InstallationType + "', version is '" + installation.Version + "'.");
			}
			return installation;
		}
		public string CreatePortableLicense(int installationID) {
			var installation = this.GetInstallation(installationID);
			if (!installation.Portable) return null;
			var licenseData = CreateLicenseData(installation.Project, installation.InstallationType, false);
			return licenseData.EncryptPortable(installation.Project.ContractNo, installation.Project.ProjectGuid, installation.InstallationType, installation.Version);
		}
		private LicenseData CreateLicenseData(Project project, string installationType, bool isDeveloper) {
			var expiryDateFromProject = project.ExpiryDate;
			var prodLicenseCountFromProject = project.ProductiveLicenseCount;
			var additiveLicenseCount = 0;
			if (project.XtendedLicenseCount > 0 && project.XtendedStartDate <= DateTime.Now.Date && project.XtendedFinishDate >= DateTime.Now.Date) {
				expiryDateFromProject = project.XtendedFinishDate;
				prodLicenseCountFromProject += project.XtendedLicenseCount;
				additiveLicenseCount = project.XtendedLicenseCount;
			}
			var licenseData = new LicenseData {
				ExpiryDate = !isDeveloper ?
			expiryDateFromProject :
			DateTime.Today.AddMonths(1)
			};
			if (project.ExpiryInMonths.HasValue) {
				var expiryDate = DateTime.Today.AddMonths(project.ExpiryInMonths.Value);
				if (expiryDate < licenseData.ExpiryDate) licenseData.ExpiryDate = expiryDate;
			}
			licenseData.LicenseCount =
				installationType == "PROD" ?
				prodLicenseCountFromProject :
				project.TestLicenseCount;
			licenseData.UserData = project.UserData;
			var moduleProperties = this.GetProjectModuleProperties(project.ID);
			foreach (var property in moduleProperties) {
				licenseData.Modules.Add(new LicenseModule {
					ModuleID = property.ModuleProperty.PropertyName,
					LicenseCount = installationType == "PROD" ?
							property.ProductiveLicenseCount + additiveLicenseCount :
							property.TestLicenseCount
				});
			}
			return licenseData;
		}

		// Logs
		public ProjectLog GetLogEntry(int? customerID, MessageTypes type, string message) {
			if (!customerID.HasValue) return null;
			return GetLogEntry(customerID, null, null, type, message);
		}
		public ProjectLog GetLogEntry(int? customerID, int? projectID, MessageTypes type, string message) {
			if (!customerID.HasValue && !projectID.HasValue) return null;
			return GetLogEntry(customerID, projectID, null, type, message);
		}
		public ProjectLog GetLogEntry(int? customerID, int? projectID, int? installationID, MessageTypes type, string message) {
			if (!customerID.HasValue && !projectID.HasValue && !installationID.HasValue) return null;
			var query = Session.Query<ProjectLog, ProjectLogMapping>();
			query.Where(pl => pl.Main.CreateDate > DateTime.Now.AddDays(-1));
			query.Where(pl => pl.Main.MessageType == type.ToString());
			if (!string.IsNullOrEmpty(message)) query.Where(pl => pl.Main.Message == message);

			if (projectID.HasValue && installationID.HasValue) {
				query.Where(pl => pl.Main.ProjectID == projectID.Value & pl.Main.InstallationID == installationID.Value);
			} else if (projectID.HasValue && !installationID.HasValue) {
				query.Where(pl => pl.Main.ProjectID == projectID.Value);
			} else if (!projectID.HasValue && installationID.HasValue) {
				query.Where(pl => pl.Main.InstallationID == installationID.Value);
			}
			query.OrderBy(pl => pl.Main.EventTime.Descending());
			return query.ReadFirst();
		}
		public List<ProjectLog> GetLogEntries(int? customerID, int? projectID, int? installationID, string criteria) {
			if (!customerID.HasValue && !projectID.HasValue && !installationID.HasValue) return null;
			if (string.IsNullOrEmpty(criteria)) criteria = string.Empty;
			criteria = string.Concat("%", criteria.ToUpper(), "%");
			var query = Session.Query<ProjectLog, ProjectLogMapping>();
			if (projectID.HasValue && installationID.HasValue) {
				query.Where(pl => pl.Main.ProjectID == projectID.Value & pl.Main.InstallationID == installationID.Value & (pl.Main.Message.ToUpper().Like(criteria) | pl.Main.MessageLong.ToUpper().Like(criteria)));
			} else if (projectID.HasValue && !installationID.HasValue) {
				query.Where(pl => pl.Main.ProjectID == projectID.Value & (pl.Main.Message.ToUpper().Like(criteria) | pl.Main.MessageLong.ToUpper().Like(criteria)));
			} else if (!projectID.HasValue && installationID.HasValue) {
				query.Where(pl => pl.Main.InstallationID == installationID.Value & (pl.Main.Message.ToUpper().Like(criteria) | pl.Main.MessageLong.ToUpper().Like(criteria)));
			}
			query.OrderBy(pl => pl.Main.EventTime.Descending());
			return query.ReadList();
		}

		public void LogEntry(int? customerID, MessageTypes type, string message) {
			LogEntry(customerID, null, null, type, message, null, null);
		}
		public void LogEntry(int? customerID, MessageTypes type, string message, string messageLong) {
			LogEntry(customerID, null, null, type, message, messageLong, null);
		}
		public void LogEntry(int? customerID, MessageTypes type, string message, string messageLong, string stackTrace) {
			LogEntry(customerID, null, null, type, message, messageLong, stackTrace);
		}
		public void LogEntry(int? customerID, int? projectID, MessageTypes type, string message) {
			LogEntry(customerID, projectID, null, type, message, null, null);
		}
		public void LogEntry(int? customerID, int? projectID, MessageTypes type, string message, string messageLong) {
			LogEntry(customerID, projectID, null, type, message, messageLong, null);
		}
		public void LogEntry(int? customerID, int? projectID, MessageTypes type, string message, string messageLong, string stackTrace) {
			LogEntry(customerID, projectID, null, type, message, messageLong, stackTrace);
		}
		public void LogEntry(int? customerID, int? projectID, int? installationID, MessageTypes type, string message) {
			LogEntry(customerID, projectID, installationID, type, message, null, null);
		}
		public void LogEntry(int? customerID, int? projectID, int? installationID, MessageTypes type, string message, string messageLong) {
			LogEntry(customerID, projectID, installationID, type, message, messageLong, null);
		}
		public void LogEntry(int? customerID, int? projectID, int? installationID, MessageTypes type, string message, string messageLong, string stackTrace) {
			var entry = new ProjectLog {
				CustomerID = customerID.GetValueOrDefault(),
				ProjectID = projectID.GetValueOrDefault(),
				InstallationID = installationID.GetValueOrDefault(),
				EventTime = DateTime.Now,
				UserName = Thread.CurrentPrincipal != null ? Thread.CurrentPrincipal.Identity.Name : "REST",
				MessageType = type.ToString(),
				Message = message,
				MessageLong = messageLong,
				Stacktrace = stackTrace,
			};

			ProjectLog lastEntry = null;
			if (customerID.HasValue && projectID.HasValue && installationID.HasValue) {
				lastEntry = GetLogEntry(customerID, projectID, installationID, type, message);
			} else if (customerID.HasValue && projectID.HasValue && !installationID.HasValue) {
				lastEntry = GetLogEntry(customerID, projectID, null, type, message);
			} else if (customerID.HasValue && !projectID.HasValue && installationID.HasValue) {
				lastEntry = GetLogEntry(customerID, null, installationID, type, message);
			}
			if (lastEntry == null) {
				Session.Insert(entry);
				switch (type) {
					case MessageTypes.InstallationExported:
						Customer customer = null;
						Project project = null;
						Installation installation = null;
						if (customerID.HasValue) customer = GetCustomer(customerID.Value);
						if (projectID.HasValue) project = GetProject(projectID.Value);
						if (installationID.HasValue) installation = GetInstallation(installationID.Value);

						var smtpClient = NetworkConfiguration.CreateSmtpClient();
						using (MailMessage mailMessage = new MailMessage()) {
							mailMessage.From = new MailAddress(NetworkConfiguration.MailFrom);
							var eMailRecipients = String.IsNullOrEmpty(installation.Project.EMail) ? customer.DefaultEMail : installation.Project.EMail;
							foreach (var r in eMailRecipients.Split(',')) {
								if (!string.IsNullOrEmpty(r)) mailMessage.To.Add(r);
							}
							mailMessage.Subject = "License-Management Message '" + type.ToString() + "' at '" + DateTime.Now.ToString() + "'.";
							mailMessage.Body = "The License-Management produced the following message:\n" +
								message + "\n" +
								messageLong + "\n";
							mailMessage.Body += customer == null ? string.Empty : "linked to Customer '" + customer.Name1 + "' (" + customer.ID + ")\n";
							mailMessage.Body += project == null ? string.Empty : "linked to Project '" + project.Description + "' (" + project.ContractNo + ")\n";
							mailMessage.Body += installation == null ? string.Empty : "linked to Installation '" + installation.InstallationType + "' (" + installation.ID + ")\n";
							mailMessage.Body += "linked to User '" + entry.UserName + "'";
							try {
								smtpClient.Send(mailMessage);
							} catch { }
						}
						break;
				}
			}
		}
	}
}
