namespace Crm.BusinessRules.PersonRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class ParentIdRequired : RequiredRule<Person>
	{
		public ParentIdRequired()
		{
			Init(p => p.ParentId);
		}
	}
}