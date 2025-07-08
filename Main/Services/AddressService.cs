namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model;
	using Crm.Services.Interfaces;

	public class AddressService : IAddressService
	{
		private readonly IRepositoryWithTypedId<Address, Guid> addressRepository;
		private readonly IRepositoryWithTypedId<Contact, Guid> contactRepository;

		public virtual Address GetAddress(Guid addressId)
		{
			return addressRepository.Get(addressId);
		}

		public virtual IQueryable<Address> GetAddresses()
		{
			return addressRepository.GetAll();
		}

		public virtual void SaveAddress(Address address)
		{
			var contact = address.CompanyId.HasValue ? contactRepository.Get(address.CompanyId.Value) : null;

			if (contact != null && contact.Addresses.Count == 0)
			{
				address.IsCompanyStandardAddress = true;
			}
			address.IsExported = false;
			addressRepository.SaveOrUpdate(address);
		}

		public virtual void DeleteAddress(Guid addressId)
		{
			var address = addressRepository.Get(addressId);
			DeleteAddress(address);
		}

		public virtual void DeleteAddress(Address address)
		{
			// ToDo: Not allowed to delete standard address. Can e.g. happen if user has an open address edit tab and meanwhile another user 
			// makes this address the standard address. I.e. this should be a very rare case. But maybe error handling should be improved so the user does not see an exception.
			if (address != null && address.IsCompanyStandardAddress)
			{
				throw new ApplicationException("Standard address cannot be deleted.");
			}

			addressRepository.Delete(address);
		}

		public virtual void SetExportedFlag(Guid addressId)
		{
			var address = GetAddress(addressId);
			address.IsExported = true;
			addressRepository.SaveOrUpdate(address);
		}

		public virtual IEnumerable<string> GetUsedAddressTypes()
		{
			return addressRepository.GetAll().Select(c => c.AddressTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedRegions()
		{
			return addressRepository.GetAll().Select(c => c.RegionKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedCountries()
		{
			return addressRepository.GetAll().Select(c => c.CountryKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedLanguages()
		{
			return addressRepository.GetAll().Select(c => c.LanguageKey).Distinct();
		}

		public AddressService(IRepositoryWithTypedId<Address, Guid> addressRepository, IRepositoryWithTypedId<Contact, Guid> contactRepository)
		{
			this.addressRepository = addressRepository;
			this.contactRepository = contactRepository;
		}
	}
}
