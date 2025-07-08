namespace Crm.Services.Interfaces
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.AutoFac;

	public interface ITagService : IDependency
	{
		void AddTagToContact(Guid contactId, string tagName);
		void AddTagToContacts(IEnumerable<Guid> contactId, string tagName);
		IList<string> GetAllTags(string contactType);
		IList<string> GetTags(string tagSubstring);
		List<string> GetTagsByContactIds(IEnumerable<Guid> contactIds);
		void RemoveTagFromContact(Guid contactId, string tagName);
	}
}