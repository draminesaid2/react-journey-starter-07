namespace Crm.BusinessRules.CompanyRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class NameRequired : RequiredRule<Company>
	{
		public NameRequired()
		{
			Init(c => c.Name);
		}
	}
}