namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class AddressTypeKeyRequired : RequiredRule<Address>
	{
		// Constructor
		public AddressTypeKeyRequired()
		{
			Init(a => a.AddressTypeKey);
		}
	}
}