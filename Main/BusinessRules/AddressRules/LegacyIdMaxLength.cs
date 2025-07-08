namespace Crm.BusinessRules.AddressRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class LegacyIdMaxLength : MaxLengthRule<Address>
	{
		public LegacyIdMaxLength()
		{
			Init(a => a.LegacyId, 32);
		}
	}
}