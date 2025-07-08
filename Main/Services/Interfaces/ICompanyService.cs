namespace Crm.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Crm.Library.AutoFac;
	using Crm.Model;

	public interface ICompanyService : IDependency
	{
		bool DoesCompanyExist(Guid companyId);
		IQueryable<Company> GetCompanies();
		Company GetCompany(Guid companyId);
		void SaveCompany(Company company);
		void DeleteCompany(Guid companyId);
		IEnumerable<string> GetUsedCompanyGroupFlag1s();
		IEnumerable<string> GetUsedCompanyGroupFlag2s();
		IEnumerable<string> GetUsedCompanyGroupFlag3s();
		IEnumerable<string> GetUsedCompanyGroupFlag4s();
		IEnumerable<string> GetUsedCompanyGroupFlag5s();
		IEnumerable<string> GetUsedCompanyTypes();
		IEnumerable<string> GetUsedNumberOfEmployeess();
		IEnumerable<string> GetUsedTurnovers();
	}
}
