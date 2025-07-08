namespace Crm.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Model;

	public interface ICommunicationService : IDependency
	{
		IQueryable<Communication> GetCommunications();
		void SetExportedFlag(Guid communicationId);
		IEnumerable<string> GetUsedEmailTypes();
		IEnumerable<string> GetUsedFaxTypes();
		IEnumerable<string> GetUsedPhoneTypes();
		IEnumerable<string> GetUsedWebsiteTypes();
		IEnumerable<string> GetUsedCountries();
	}
}
