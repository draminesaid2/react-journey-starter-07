namespace Crm.BusinessRules.BravoRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class IssueMaxLength : MaxLengthRule<Bravo>
	{
		public IssueMaxLength()
		{
			Init(b => b.Issue, 500);
		}
	}
}