using System;
using System.Collections.Generic;
using LMobile.Gen3LicenseManagement.Dao.BusinessObjects;

namespace LMobile.Gen3LicenseManagement.Dao.Contracts {
	public interface ILicenseDao {
		List<Customer> GetCustomers(string searchKey);
		List<Customer> GetCustomersByContractNo(string p_ContractNo);
		List<Customer> GetCustomersByPropertyName(string p_PropertyName);
		Customer GetCustomer(int id);
		Customer GetCustomer(string customerNo);
		List<Project> GetProjectsByCustomer(int customerID);
		Project GetProject(int id);
		Project GetProject(string contractNo);

		List<Installation> GetByKeyword(string keyWord);
		List<Installation> GetInstallationsByCustomer(int customerID);
		List<Installation> GetInstallationsByProject(int projectID);
		Installation GetInstallation(int ID);
		Installation ImportLicenseRequest(string request);
		Installation ImportLicenseRequest(string request, out LicenseRequest licenseRequest);
		Installation ImportLicenseRequest(string request, out LicenseRequest licenseRequest, out bool isDeveloper);
		string CreateLicenseForRequest(string request);

		Installation CreatePortableInstallation(int projectID, string installationType, int version);
		string CreatePortableLicense(int installationID);

		List<ProjectModuleProperty> GetProjectModuleProperties(int projectID);
		ProjectModuleProperty GetProjectModuleProperty(int projectModulePropertyID);

		List<StoredProjectType> GetProjectTypes();
		List<StoredInstallationType> GetInstallationTypes();

		ProjectLog GetLogEntry(int? customerID, MessageTypes type, string message);
		ProjectLog GetLogEntry(int? customerID, int? projectID, MessageTypes type, string message);
		ProjectLog GetLogEntry(int? customerID, int? projectID, int? installationID, MessageTypes type, string message);
		List<ProjectLog> GetLogEntries(int? customerID, int? projectID, int? installationID, string criteria);

		void LogEntry(int? customerID, MessageTypes type, string message);
		void LogEntry(int? customerID, MessageTypes type, string message, string messageLong);
		void LogEntry(int? customerID, MessageTypes type, string message, string messageLong, string stackTrace);
		void LogEntry(int? customerID, int? projectID, MessageTypes type, string message);
		void LogEntry(int? customerID, int? projectID, MessageTypes type, string message, string messageLong);
		void LogEntry(int? customerID, int? projectID, MessageTypes type, string message, string messageLong, string stackTrace);
		void LogEntry(int? customerID, int? projectID, int? installationID, MessageTypes type, string message);
		void LogEntry(int? customerID, int? projectID, int? installationID, MessageTypes type, string message, string messageLong);
		void LogEntry(int? customerID, int? projectID, int? installationID, MessageTypes type, string message, string messageLong, string stackTrace);
	}
}
