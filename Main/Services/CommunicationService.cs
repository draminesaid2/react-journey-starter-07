namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model;
	using Crm.Services.Interfaces;

	public class CommunicationService : ICommunicationService
	{
		private readonly IRepositoryWithTypedId<Communication, Guid> communicationRepository;
		private readonly IRepositoryWithTypedId<Fax, Guid> faxRepository;
		private readonly IRepositoryWithTypedId<Email, Guid> emailRepository;
		private readonly IRepositoryWithTypedId<Phone, Guid> phoneRepository;
		private readonly IRepositoryWithTypedId<Website, Guid> websiteRepository;

		public virtual IQueryable<Communication> GetCommunications()
		{
			return communicationRepository.GetAll();
		}

		public virtual Communication GetCommunication(Guid communicationId)
		{
			return communicationRepository.Get(communicationId);
		}

		public virtual void SetExportedFlag(Guid communicationId)
		{
			var communication = GetCommunication(communicationId);
			communication.IsExported = true;
			communicationRepository.SaveOrUpdate(communication);
		}

		public virtual IEnumerable<string> GetUsedEmailTypes()
		{
			return emailRepository.GetAll().Select(c => c.TypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedFaxTypes()
		{
			return faxRepository.GetAll().Select(c => c.TypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedPhoneTypes()
		{
			return phoneRepository.GetAll().Select(c => c.TypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedWebsiteTypes()
		{
			return websiteRepository.GetAll().Select(c => c.TypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedCountries()
		{
			return communicationRepository.GetAll().Select(c => c.CountryKey).Distinct();
		}

		public CommunicationService(IRepositoryWithTypedId<Communication, Guid> communicationRepository,
			IRepositoryWithTypedId<Fax, Guid> faxRepository,
			IRepositoryWithTypedId<Email, Guid> emailRepository,
			IRepositoryWithTypedId<Phone, Guid> phoneRepository,
			IRepositoryWithTypedId<Website, Guid> websiteRepository)
		{
			this.communicationRepository = communicationRepository;
			this.faxRepository = faxRepository;
			this.emailRepository = emailRepository;
			this.phoneRepository = phoneRepository;
			this.websiteRepository = websiteRepository;
		}
	}
}
