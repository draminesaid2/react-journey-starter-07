namespace Crm.BusinessRules.TaskRules
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Validation;
	using Crm.Model;

	public class ContactIdMustExist : Rule<Task>
	{
		private readonly IRepositoryWithTypedId<Contact, Guid> contactRepository;

		// Methods
		protected override bool IsIgnoredFor(Task task)
		{
			return task.ContactId == null;
		}

		public override bool IsSatisfiedBy(Task task)
		{
			return contactRepository.GetAll().Any(x => x.Id == task.ContactId.Value);
		}

		protected override RuleViolation CreateRuleViolation(Task task)
		{
			return RuleViolation(task, t => t.ContactId, "Contact");
		}

		// Constructor
		public ContactIdMustExist(IRepositoryWithTypedId<Contact, Guid> contactRepository)
			: base(RuleClass.MustExist)
		{
			this.contactRepository = contactRepository;
		}
	}
}
