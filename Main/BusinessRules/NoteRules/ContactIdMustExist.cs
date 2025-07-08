namespace Crm.BusinessRules.NoteRules
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Validation;
	using Crm.Model;
	using Crm.Model.Notes;

	// This rule checks if a contact with the given ContactId exists. This rule can be violated if user A deletes a contact while another user B has the details page for this contact
	// open and tries to create a note for the no longer existing contact.
	public class ContactIdMustExist : Rule<Note>
	{
		private readonly IRepositoryWithTypedId<Contact, Guid> repository;

		// Methods
		protected override bool IsIgnoredFor(Note note)
		{
			return note.ContactId == null;
		}

		public override bool IsSatisfiedBy(Note note)
		{
			return repository.GetAll().Any(x => x.Id == note.ContactId.Value);
		}

		protected override RuleViolation CreateRuleViolation(Note note)
		{
			return RuleViolation(note, n => n.ContactId, "Contact");
		}

		// Constructor
		public ContactIdMustExist(IRepositoryWithTypedId<Contact, Guid> repository)
			: base(RuleClass.MustExist)
		{
			this.repository = repository;
		}
	}
}
