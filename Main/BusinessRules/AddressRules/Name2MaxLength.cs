namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class Name2MaxLength : MaxLengthRule<Address>
	{
		public Name2MaxLength()
		{
			Init(a => a.Name2, 180);
		}
	}
}