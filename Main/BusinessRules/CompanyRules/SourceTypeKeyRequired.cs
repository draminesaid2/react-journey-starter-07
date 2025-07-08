namespace Crm.BusinessRules.CompanyRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class SourceTypeKeyRequired : RequiredRule<Company>
	{
		public SourceTypeKeyRequired()
		{
			Init(c => c.SourceTypeKey);
		}
	}
}