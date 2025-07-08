namespace Crm.BusinessRules.BravoRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class IssueRequired : RequiredRule<Bravo>
	{
		public IssueRequired()
		{
			Init(b => b.Issue);
		}
	}
}