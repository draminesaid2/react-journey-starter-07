namespace Crm.BusinessRules.PersonRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class FirstnameMaxLength : MaxLengthRule<Person>
	{
		public FirstnameMaxLength()
		{
			Init(c => c.Firstname, 80);
		}
	}
}