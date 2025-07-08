namespace Crm.BusinessRules.CompanyRules
{
	using System;

	using Crm.Library.Validation;
	using Crm.Model;

	public class ParentIsNotCurrent : Rule<Company>
	{
		public override bool IsSatisfiedBy(Company company)
		{
			if (!company.ParentId.HasValue || company.ParentId.Value == Guid.Empty)
				return true;

			return company.Id != company.ParentId.Value;
		}
		protected override RuleViolation CreateRuleViolation(Company company)
		{
			return RuleViolation(company, c => c.ParentId, "ParentCompany", "CompanyIsCurrentCompany");
		}
		public ParentIsNotCurrent()
			: base(RuleClass.Undefined)
		{
		}
	}
}