namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class POBoxMaxLength : MaxLengthRule<Address>
	{
		public POBoxMaxLength()
		{
			Init(a => a.POBox, 35);
		}
	}
}