namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class ZipCodeRequired : RequiredRule<Address>
	{
		public ZipCodeRequired()
		{
			Init(a => a.ZipCode);
		}
	}
}