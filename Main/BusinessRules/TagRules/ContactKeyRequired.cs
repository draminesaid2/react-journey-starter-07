namespace Crm.BusinessRules.TagRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class ContactKeyRequired : RequiredRule<Tag>
	{
		public ContactKeyRequired()
		{
			Init(x => x.ContactKey, "Contact");
		}
	}
}