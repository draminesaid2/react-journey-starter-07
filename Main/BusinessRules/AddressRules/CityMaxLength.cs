namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class CityMaxLength : MaxLengthRule<Address>
	{
		public CityMaxLength()
		{
			Init(a => a.City, 80);
		}
	}
}