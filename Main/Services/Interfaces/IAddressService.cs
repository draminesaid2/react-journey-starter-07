namespace Crm.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Model;

	public interface IAddressService : ITransientDependency
	{
		Address GetAddress(Guid addressId);
		IQueryable<Address> GetAddresses();
		void SaveAddress(Address address);
		void DeleteAddress(Guid addressId);
		void SetExportedFlag(Guid addressId);
		IEnumerable<string> GetUsedAddressTypes();
		IEnumerable<string> GetUsedRegions();
		IEnumerable<string> GetUsedCountries();
		IEnumerable<string> GetUsedLanguages();
	}
}
