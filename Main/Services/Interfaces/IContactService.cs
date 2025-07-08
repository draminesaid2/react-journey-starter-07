namespace Crm.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using Crm.Library.AutoFac;
	using Crm.Model;

	public interface IContactService : ITransientDependency
	{
		bool DoesContactExist(Guid id);
		void SaveContact(Contact contact);
		void SaveCommunications<TCommunication>(IEnumerable<TCommunication> communications, Guid contactId, Guid? addressId)
			where TCommunication : Communication;
		void SetExportedFlag(Guid contactId);
		IEnumerable<string> GetUsedLanguages();
	}
}
