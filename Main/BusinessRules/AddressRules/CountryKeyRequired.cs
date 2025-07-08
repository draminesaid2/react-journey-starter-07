namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class CountryKeyRequired : RequiredRule<Address>
	{
		public CountryKeyRequired()
		{
			Init(a => a.CountryKey);
		}
	}
}