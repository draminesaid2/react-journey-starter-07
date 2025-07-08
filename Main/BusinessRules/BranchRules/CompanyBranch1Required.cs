namespace Crm.BusinessRules.BranchRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class CompanyBranch1Required : RequiredRule<CompanyBranch>
	{
		public CompanyBranch1Required()
		{
			Init(e => e.Branch1Key, "Branch1Key");
		}
	}
}