namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Crm.Events;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Events;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Services.Interfaces;

	public class PersonService : IPersonService
	{
		private readonly IContactService contactService;
		private readonly IUserService userService;
		private readonly IRepositoryWithTypedId<Person, Guid> personRepository;
		private readonly IEventAggregator eventAggregator;

		// Methods
		public virtual IQueryable<Person> GetPersons()
		{
			return personRepository.GetAll();
		}
		
		public virtual Person GetPerson(Guid personId)
		{
			return personRepository.Get(personId);
		}

		public virtual void SavePerson(Person person)
		{
			var phones = person.Phones;
			var emails = person.Emails;
			var faxes = person.Faxes;
			var websites = person.Websites;

			person.Communications.Clear();
			person.IsExported = false;
			personRepository.SaveOrUpdate(person);
			phones.ForEach(p => p.AddressId = person.Address.Id);
			emails.ForEach(e => e.AddressId = person.Address.Id);
			faxes.ForEach(f => f.AddressId = person.Address.Id);
			websites.ForEach(w => w.AddressId = person.Address.Id);
			contactService.SaveCommunications(phones, person.Id, person.Address.Id);
			contactService.SaveCommunications(emails, person.Id, person.Address.Id);
			contactService.SaveCommunications(faxes, person.Id, person.Address.Id);
			contactService.SaveCommunications(websites, person.Id, person.Address.Id);
		}

		public virtual void DeletePerson(Guid personId)
		{
			var person = personRepository.Get(personId);
			person.IsActive = false;
			person.InactiveDate = DateTime.UtcNow;
			person.InactiveUser = userService.CurrentUser.Id;
			personRepository.SaveOrUpdate(person);
			eventAggregator.Publish(new PersonDeletedEvent(person.Id));
		}

		public virtual IEnumerable<string> GetUsedBusinessTitles()
		{
			return personRepository.GetAll().Select(c => c.BusinessTitleKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedDepartmentTypes()
		{
			return personRepository.GetAll().Select(c => c.DepartmentTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedSalutations()
		{
			return personRepository.GetAll().Select(c => c.SalutationKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedSalutationLetters()
		{
			return personRepository.GetAll().Select(c => c.SalutationLetterKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedTitles()
		{
			return personRepository.GetAll().Select(c => c.TitleKey).Distinct();
		}

		public PersonService(IContactService contactService, IUserService userService, IRepositoryWithTypedId<Person, Guid> personRepository, IEventAggregator eventAggregator)
		{
			this.contactService = contactService;
			this.userService = userService;
			this.personRepository = personRepository;
			this.eventAggregator = eventAggregator;
		}
	}
}
