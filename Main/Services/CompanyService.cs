namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Crm.Events;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Modularization.Events;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Services.Interfaces;

	public class CompanyService : ICompanyService, IEventHandler<AddressDeletedEvent>
	{
		private readonly IContactService contactService;
		private readonly IUserService userService;
		private readonly IRepositoryWithTypedId<Company, Guid> companyRepository;
		private readonly IEventAggregator eventAggregator;

		// Methods
		public virtual bool DoesCompanyExist(Guid companyId)
		{
			return GetCompanies().Any(c => c.Id == companyId);
		}

		public virtual IQueryable<Company> GetCompanies()
		{
			return companyRepository.GetAll();
		}
		
		public virtual Company GetCompany(Guid companyId)
		{
			return companyRepository.Get(companyId);
		}

		public virtual void SaveCompany(Company company)
		{
			contactService.SaveContact(company);
		}

		public virtual void DeleteCompany(Guid companyId)
		{
			var company = GetCompany(companyId);
			company.IsActive = false;
			company.InactiveDate = DateTime.UtcNow;
			company.InactiveUser = userService.CurrentUser.Id;
			companyRepository.SaveOrUpdate(company);
			eventAggregator.Publish(new CompanyDeletedEvent(company.Id));
		}

	public virtual void Handle(AddressDeletedEvent e)
		{
			if (e.Address.IsCompanyStandardAddress && e.Address.CompanyId.HasValue)
			{
				var company = GetCompany(e.Address.CompanyId.Value);

				if (company == null || company.Addresses.None())
				{
					return;
				}

				company.Addresses.First().IsCompanyStandardAddress = true;

				SaveCompany(company);
			}
		}

		public virtual IEnumerable<string> GetUsedCompanyGroupFlag1s()
		{
			return companyRepository.GetAll().Select(c => c.CompanyGroupFlag1Key).Distinct();
		}

		public virtual IEnumerable<string> GetUsedCompanyGroupFlag2s()
		{
			return companyRepository.GetAll().Select(c => c.CompanyGroupFlag2Key).Distinct();
		}

		public virtual IEnumerable<string> GetUsedCompanyGroupFlag3s()
		{
			return companyRepository.GetAll().Select(c => c.CompanyGroupFlag3Key).Distinct();
		}

		public virtual IEnumerable<string> GetUsedCompanyGroupFlag4s()
		{
			return companyRepository.GetAll().Select(c => c.CompanyGroupFlag4Key).Distinct();
		}

		public virtual IEnumerable<string> GetUsedCompanyGroupFlag5s()
		{
			return companyRepository.GetAll().Select(c => c.CompanyGroupFlag5Key).Distinct();
		}

		public virtual IEnumerable<string> GetUsedCompanyTypes()
		{
			return companyRepository.GetAll().Select(c => c.CompanyTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedNumberOfEmployeess()
		{
			return companyRepository.GetAll().Select(c => c.NumberOfEmployeesKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedTurnovers()
		{
			return companyRepository.GetAll().Select(c => c.TurnoverKey).Distinct();
		}

		public CompanyService(IContactService contactService, IRepositoryWithTypedId<Company, Guid> companyRepository, IUserService userService, IEventAggregator eventAggregator)
		{
			this.contactService = contactService;
			this.companyRepository = companyRepository;
			this.userService = userService;
			this.eventAggregator = eventAggregator;
		}
	}
}
