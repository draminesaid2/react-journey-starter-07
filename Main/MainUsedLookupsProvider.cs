namespace Crm
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Globalization;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Model.Lookup;
	using Crm.Library.Services.Interfaces;
	using Crm.Model.Lookups;
	using Crm.Model.Relationships;
	using Crm.Services.Interfaces;

	public class MainUsedLookupsProvider : IUsedLookupsProvider
	{
		private readonly ITaskService taskService;
		private readonly IAddressService addressService;
		private readonly IUserService userService;
		private readonly IBravoService bravoService;
		private readonly ICommunicationService communicationService;
		private readonly IRepositoryWithTypedId<BusinessRelationship, Guid> businessRelationshipRepository;
		private readonly IRepositoryWithTypedId<CompanyPersonRelationship, Guid> companyPersonRelationshipRepository;
		private readonly ICompanyService companyService;
		private readonly IPersonService personService;
		private readonly IContactService contactService;
		public MainUsedLookupsProvider(ITaskService taskService, IUserService userService,
			IAddressService addressService,
			IBravoService bravoService,
			ICommunicationService communicationService,
			IRepositoryWithTypedId<BusinessRelationship, Guid> businessRelationshipRepository,
			IRepositoryWithTypedId<CompanyPersonRelationship, Guid> companyPersonRelationshipRepository,
			ICompanyService companyService,
			IPersonService personService,
			IContactService contactService)
		{
			this.taskService = taskService;
			this.userService = userService;
			this.addressService = addressService;
			this.bravoService = bravoService;
			this.communicationService = communicationService;
			this.businessRelationshipRepository = businessRelationshipRepository;
			this.companyPersonRelationshipRepository = companyPersonRelationshipRepository;
			this.companyService = companyService;
			this.personService = personService;
			this.contactService = contactService;
		}
		public virtual IEnumerable<object> GetUsedLookupKeys(Type lookupType)
		{
			if (lookupType == typeof(TaskType))
			{
				return taskService.GetUsedTaskTypes();
			}

			if (lookupType == typeof(Skill))
			{
				return userService.GetUsers().Where(x => x.SkillKeys.Any()).SelectMany(x => x.SkillKeys).Distinct();
			}

			if (lookupType == typeof(UserStatus))
			{
				return userService.GetUsers().Select(x => x.StatusKey).Distinct();
			}

			if (lookupType == typeof(BusinessRelationshipType))
			{
				return businessRelationshipRepository.GetAll().Select(c => c.RelationshipTypeKey).Distinct();
			}

			if (lookupType == typeof(CompanyPersonRelationshipType))
			{
				return companyPersonRelationshipRepository.GetAll().Select(c => c.RelationshipTypeKey).Distinct();
			}

			if (lookupType == typeof(AddressType))
			{
				return addressService.GetUsedAddressTypes();
			}

			if (lookupType == typeof(Region))
			{
				return addressService.GetUsedRegions();
			}

			if(lookupType == typeof(BravoCategory))
			{
				return bravoService.GetUsedBravoCategories();
			}

			if (lookupType == typeof(EmailType))
			{
				return communicationService.GetUsedEmailTypes();
			}

			if (lookupType == typeof(FaxType))
			{
				return communicationService.GetUsedFaxTypes();
			}

			if (lookupType == typeof(PhoneType))
			{
				return communicationService.GetUsedPhoneTypes();
			}

			if (lookupType == typeof(WebsiteType))
			{
				return communicationService.GetUsedWebsiteTypes();
			}

			if (lookupType == typeof(CompanyGroupFlag1))
			{
				return companyService.GetUsedCompanyGroupFlag1s();
			}

			if (lookupType == typeof(CompanyGroupFlag2))
			{
				return companyService.GetUsedCompanyGroupFlag2s();
			}

			if (lookupType == typeof(CompanyGroupFlag3))
			{
				return companyService.GetUsedCompanyGroupFlag3s();
			}

			if (lookupType == typeof(CompanyGroupFlag4))
			{
				return companyService.GetUsedCompanyGroupFlag4s();
			}

			if (lookupType == typeof(CompanyGroupFlag5))
			{
				return companyService.GetUsedCompanyGroupFlag5s();
			}

			if (lookupType == typeof(CompanyType))
			{
				return companyService.GetUsedCompanyTypes();
			}

			if (lookupType == typeof(NumberOfEmployees))
			{
				return companyService.GetUsedNumberOfEmployeess();
			}

			if (lookupType == typeof(Turnover))
			{
				return companyService.GetUsedTurnovers();
			}

			if (lookupType == typeof(BusinessTitle))
			{
				return personService.GetUsedBusinessTitles();
			}

			if (lookupType == typeof(DepartmentType))
			{
				return personService.GetUsedDepartmentTypes();
			}

			if (lookupType == typeof(Salutation))
			{
				return personService.GetUsedSalutations();
			}

			if (lookupType == typeof(SalutationLetter))
			{
				return personService.GetUsedSalutationLetters();
			}

			if (lookupType == typeof(Title))
			{
				return personService.GetUsedTitles();
			}

			if (lookupType == typeof(Country))
			{
				return addressService.GetUsedCountries().Union(communicationService.GetUsedCountries());
			}

			if (lookupType == typeof(Language))
			{
				var usedLanguages = userService.GetUsers().Select(x => x.DefaultLanguageKey).Distinct();
				return usedLanguages.Union(contactService.GetUsedLanguages())
					.Union(addressService.GetUsedLanguages());
			}

			return new List<object>();
		}
	}
}
