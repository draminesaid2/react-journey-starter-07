namespace Crm.BusinessRules.CompanyRules
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Validation;
	using Crm.Model;

	public class ParentIdMustExist : Rule<Company>
	{
		private readonly IRepositoryWithTypedId<Contact, Guid> repository;

		// Methods
		protected override bool IsIgnoredFor(Company company)
		{
			return company.ParentId == null;
		}

		public override bool IsSatisfiedBy(Company company)
		{
			if (company.ParentId.IsNullOrDefault())
				return false;

			return repository.GetAll().Any(x => x.Id == company.ParentId.Value);
		}

		protected override RuleViolation CreateRuleViolation(Company company)
		{
			return RuleViolation(company, c => c.ParentId, "ParentCompany");
		}

		// Constructor
		public ParentIdMustExist(IRepositoryWithTypedId<Contact, Guid> repository)
			: base(RuleClass.MustExist)
		{
			this.repository = repository;
		}
	}
}
