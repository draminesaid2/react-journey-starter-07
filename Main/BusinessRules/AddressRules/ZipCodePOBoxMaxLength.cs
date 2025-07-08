namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class ZipCodePOBoxMaxLength : MaxLengthRule<Address>
	{
		public ZipCodePOBoxMaxLength()
		{
			Init(a => a.ZipCodePOBox, 20);
		}
	}
}