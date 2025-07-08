namespace Crm.BusinessRules.PersonRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class SurnameRequired : RequiredRule<Person>
	{
		public SurnameRequired()
		{
			Init(p => p.Surname);
		}
	}
}