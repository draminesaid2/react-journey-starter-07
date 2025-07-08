namespace Crm.BusinessRules.CommunicationRules.FaxRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class TypeKeyRequired : RequiredRule<Fax>
	{
		// Constructor
		public TypeKeyRequired()
		{
			Init(e => e.TypeKey);
		}
	}
}