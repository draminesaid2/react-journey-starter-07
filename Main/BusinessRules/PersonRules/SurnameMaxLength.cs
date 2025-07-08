namespace Crm.BusinessRules.PersonRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class SurnameMaxLength : MaxLengthRule<Person>
	{
		public SurnameMaxLength()
		{
			Init(c => c.Surname, 80);
		}
	}
}