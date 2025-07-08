namespace Crm.BusinessRules.CompanyRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class CompanyTypeKeyRequired : RequiredRule<Company>
	{
		public CompanyTypeKeyRequired()
		{
			Init(c => c.CompanyTypeKey);
		}
	}
}