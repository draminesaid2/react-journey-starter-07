namespace Crm.BusinessRules.PersonRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class NameMaxLength : MaxLengthRule<Person>
	{
		public NameMaxLength()
		{
			Init(c => c.Name, 120);
		}
	}
}