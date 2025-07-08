using Microsoft.AspNetCore.Mvc;

namespace Crm.Rest.Controllers
{
	using System;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Model;

	public class ContactRestController : RestController<Contact>
	{
		private readonly IRepositoryWithTypedId<Contact, Guid> contactRepository;

		public ContactRestController(IRepositoryWithTypedId<Contact, Guid> contactRepository, RestTypeProvider restTypeProvider)
			: base(restTypeProvider)
		{
			this.contactRepository = contactRepository;
		}

		public virtual ActionResult Get(Guid id)
		{
			var contact = contactRepository.Get(id);
			return Rest(contact);
		}
	}
}
