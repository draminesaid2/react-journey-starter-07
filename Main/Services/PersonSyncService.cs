namespace Crm.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Services.Interfaces;

	using NHibernate.Linq;

	public class PersonSyncService : DefaultSyncService<Person, Guid>
	{
		private readonly IRepositoryWithTypedId<Address, Guid> addressRepository;
		private readonly IRepositoryWithTypedId<Communication, Guid> communicationRepository;
		private readonly IPersonService personService;
		private readonly IVisibilityProvider visibilityProvider;

		public PersonSyncService(
			IRepositoryWithTypedId<Person, Guid> repository,
			RestTypeProvider restTypeProvider,
			IRestSerializer restSerializer,
			IRepositoryWithTypedId<Address, Guid> addressRepository,
			IRepositoryWithTypedId<Communication, Guid> communicationRepository,
			IPersonService personService,
			IMapper mapper,
			IVisibilityProvider visibilityProvider)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		    this.addressRepository = addressRepository;
			this.communicationRepository = communicationRepository;
			this.personService = personService;
			this.visibilityProvider = visibilityProvider;
			this.communicationRepository = communicationRepository;
		}
		public override Type[] SyncDependencies => new[] { typeof(Address), typeof(Company) };
	    public override Person Save(Person entity)
		{
			entity.Address = addressRepository.Get(entity.StandardAddressKey);
			entity.Communications = communicationRepository.GetAll().Where(x => x.ContactId == entity.Id).ToList();
			personService.SavePerson(entity);
			return entity;
		}
		public override IQueryable<Person> GetAll(User user)
		{
			return visibilityProvider.FilterByVisibility(repository.GetAll());
		}

		public override IQueryable<Person> Eager(IQueryable<Person> entities)
		{
			entities = entities
				.Fetch(x => x.Address)
				.Fetch(x => x.Parent);
			return entities;
		}
	}
}