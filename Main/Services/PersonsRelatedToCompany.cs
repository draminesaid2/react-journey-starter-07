namespace Crm.Services
{
	using System;
	using System.Linq;

	using Crm.Controllers;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model;

	public class PersonsRelatedToCompany : IRelatedContact<Company>
	{
		private readonly IRepositoryWithTypedId<Person, Guid> personRepository;
		public PersonsRelatedToCompany(IRepositoryWithTypedId<Person, Guid> personRepository)
		{
			this.personRepository = personRepository;
		}
		public virtual IQueryable<Contact> RelatedContact(Contact contact)
		{
			var contacts = personRepository.GetAll().Where(x => x.ParentId == contact.Id).Select(y => y);

			return contacts;
		}
	}
}
