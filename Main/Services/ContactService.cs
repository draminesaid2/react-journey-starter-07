namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Crm.Events;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Events;
	using Crm.Model;
	using Crm.Services.Interfaces;

	[Serializable]
	public class ContactService : IContactService, IEventHandler<AddressDeletedEvent>
	{
		private readonly IRepositoryWithTypedId<Contact, Guid> contactRepository;
		private readonly IRepositoryWithTypedId<Communication, Guid> communicationRepository;
		private readonly IAddressService addressService;

		public virtual void SaveContact(Contact contact)
		{
			var phones = contact.Phones;
			var emails = contact.Emails;
			var faxes = contact.Faxes;
			var websites = contact.Websites;

			contact.Communications.Clear();
			contact.IsExported = false;
			contactRepository.SaveOrUpdate(contact);

			if (contact.StandardAddress != null)
			{
				contact.StandardAddress.LanguageKey = contact.LanguageKey;
				contact.StandardAddress.CompanyId = contact.Id;
				addressService.SaveAddress(contact.StandardAddress);
				SaveCommunications(emails, contact.Id, contact.StandardAddress.Id);
				SaveCommunications(phones, contact.Id, contact.StandardAddress.Id);
				SaveCommunications(faxes, contact.Id, contact.StandardAddress.Id);
				SaveCommunications(websites, contact.Id, contact.StandardAddress.Id);
			}
		}

		public virtual void SaveCommunications<TCommunication>(IEnumerable<TCommunication> communications, Guid contactId, Guid? addressId)
			where TCommunication : Communication
		{
			communications = communications == null ? new List<TCommunication>() : communications.ToList();

			foreach (var communication in communications)
			{
				communication.ContactId = contactId;
				communication.AddressId = addressId;
			}

			var communicationIds = communications.Select(c => c.Id);
			var allCommunications = communicationRepository.GetAll().OfType<TCommunication>()
																										 .Where(c => c.ContactId == contactId)
																										 .Where(c => c.AddressId == addressId)
																										 .ToList();
			var communicationsToDelete = allCommunications.Where(c => !communicationIds.Contains(c.Id));
			foreach (var communication in communicationsToDelete)
			{
				communicationRepository.Delete(communication);
			}

			foreach (var communication in communications)
			{
				communication.IsExported = false;
				communicationRepository.SaveOrUpdate(communication);
			}
		}

		public virtual void Handle(AddressDeletedEvent e)
		{
			foreach (Communication communication in communicationRepository.GetAll().Where(c => c.AddressId == e.Address.Id))
			{
				communicationRepository.Delete(communication);
			}
		}

		public virtual bool DoesContactExist(Guid id)
		{
			return contactRepository.Get(id) != null;
		}

		public virtual void SetExportedFlag(Guid contactId)
		{
			var contact = contactRepository.Get(contactId);
			contact.IsExported = true;
			contactRepository.SaveOrUpdate(contact);
		}

		public virtual IEnumerable<string> GetUsedLanguages()
		{
			return contactRepository.GetAll().Select(c => c.LanguageKey).Distinct();
		}

		// Constructor
		public ContactService(
			IRepositoryWithTypedId<Contact, Guid> contactRepository,
			IRepositoryWithTypedId<Communication, Guid> communicationRepository, 
			IAddressService addressService)
		{
			this.contactRepository = contactRepository;
			this.communicationRepository = communicationRepository;
			this.addressService = addressService;
		}
	}
}
