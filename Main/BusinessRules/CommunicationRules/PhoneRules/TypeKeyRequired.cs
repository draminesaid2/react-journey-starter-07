namespace Crm.BusinessRules.CommunicationRules.PhoneRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class TypeKeyRequired : RequiredRule<Phone>
	{
		// Constructor
		public TypeKeyRequired()
		{
			Init(e => e.TypeKey);
		}
	}
}