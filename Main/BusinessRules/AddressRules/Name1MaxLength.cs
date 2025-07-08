namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class Name1MaxLength : MaxLengthRule<Address>
	{
		public Name1MaxLength()
		{
			Init(a => a.Name1, 450);
		}
	}
}