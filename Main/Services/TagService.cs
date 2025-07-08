namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Model;
	using Crm.Services.Interfaces;

	public class TagService : ITagService
	{
		private readonly IRepository<Tag> tagRepository;
		private readonly Func<Tag> tagFactory;

		public TagService(IRepository<Tag> tagRepository, Func<Tag> tagFactory)
		{
			this.tagRepository = tagRepository;
			this.tagFactory = tagFactory;
		}
		public virtual void AddTagToContact(Guid contactId, string tagName)
		{
			AddTagToContacts(contactId.AsEnumerable(), tagName);
		}
		public virtual void AddTagToContacts(IEnumerable<Guid> contactIds, string tagName)
		{
			var counter = 0;
			var batchSize = 2000 ;
			var contactIdBatch = contactIds.Take(batchSize).ToArray();

			while (contactIdBatch.Any())
			{
				var existingTagGroups = tagRepository
					.GetAll()
					.Where(x => contactIdBatch.Contains(x.ContactKey))
					.GroupBy(x => x.ContactKey)
					.Select(g => new {g.Key, g})
					.ToArray();

				foreach (var contactId in contactIdBatch)
				{
					if (existingTagGroups.All(x => x.Key != contactId) || existingTagGroups.Where(x => x.Key == contactId).Any(x => x.g.All(tag => tag.Name != tagName)))
					{
						var tag = tagFactory();
						tag.ContactKey = contactId;
						tag.Name = tagName;
						tagRepository.SaveOrUpdate(tag);
					}
				}

				counter++;
				contactIdBatch = contactIds.Skip(counter * batchSize).Take(batchSize).ToArray();
			}
		}
		public virtual IList<string> GetAllTags(string contactType)
		{
			var tags = tagRepository.GetAll();
			if (!String.IsNullOrEmpty(contactType))
			{
				var contactTypes = contactType.Split('|');
				tags = tags.Where(t => contactTypes.Contains(t.Contact.ContactType));
			}
			return tags.Select(t => t.Name).OrderBy(t => t).Distinct().ToList();
		}
		public virtual IList<string> GetTags(string searchTag)
		{
			var tags = tagRepository.GetAll().Where(t => t.Name.IndexOf(searchTag, StringComparison.OrdinalIgnoreCase) > -1);
			var tagNames = tags.Select(t => t.Name).OrderBy(t => t).Distinct().ToList();
			return tagNames;
		}
		public virtual List<string> GetTagsByContactIds(IEnumerable<Guid> contactIds)
		{
			return tagRepository.GetAll().Where(t => contactIds.Contains(t.ContactKey)).ToList().Select(t => t.Name).ToList();
		}
		public virtual void RemoveTagFromContact(Guid contactId, string tagName)
		{
			var tag = tagRepository.GetAll().FirstOrDefault(t => t.ContactKey == contactId && t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));
			if (tag != null)
			{
				tagRepository.Delete(tag);
			}
		}
	}
}
