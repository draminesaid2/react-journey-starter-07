namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class ZipCodeMaxLength : MaxLengthRule<Address>
	{
		public ZipCodeMaxLength()
		{
			Init(a => a.ZipCode, 20);
		}
	}
}