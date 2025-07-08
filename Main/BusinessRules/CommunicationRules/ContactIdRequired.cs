namespace Crm.BusinessRules.CommunicationRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class ContactIdRequired : RequiredRule<Communication>
	{
		public ContactIdRequired()
		{
			Init(x => x.ContactId, "Contact");
		}
	}
}