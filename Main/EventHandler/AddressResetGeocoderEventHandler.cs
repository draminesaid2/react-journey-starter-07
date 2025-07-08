namespace Crm.EventHandler
{
	using System;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Events;
	using Crm.Model;

	public class AddressResetGeocoderEventHandler : IEventHandler<EntityModifiedEvent<Address>>
	{
		private readonly IRepositoryWithTypedId<Address, Guid> addressRepository;
		public AddressResetGeocoderEventHandler(IRepositoryWithTypedId<Address, Guid> addressRepository)
		{
			this.addressRepository = addressRepository;
		}

		public virtual void Handle(EntityModifiedEvent<Address> e)
		{
			var addressBefore = e.EntityBeforeChange;
			var address = e.Entity;

			if (address.Longitude.HasValue && address.Latitude.HasValue
				&& (addressBefore.City != address.City
				|| addressBefore.CountryKey != address.CountryKey
				|| addressBefore.Street != address.Street
				|| addressBefore.ZipCode != address.ZipCode))
			{
				address.GeocodingRetryCounter = 0;
				address.Longitude = null;
				address.Latitude = null;

				addressRepository.SaveOrUpdate(address);
			}
		}
	}
}
