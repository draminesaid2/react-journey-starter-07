namespace Crm.BusinessRules.CompanyRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class NameMaxLength : MaxLengthRule<Company>
	{
		public NameMaxLength()
		{
			Init(c => c.Name, 120);
		}
	}
}