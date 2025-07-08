namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class CityRequired : RequiredRule<Address>
	{
		public CityRequired()
		{
			Init(a => a.City);
		}
	}
}