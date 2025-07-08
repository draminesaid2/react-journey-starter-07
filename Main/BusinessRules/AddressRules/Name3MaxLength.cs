namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class Name3MaxLength : MaxLengthRule<Address>
	{
		public Name3MaxLength()
		{
			Init(a => a.Name3, 180);
		}
	}
}