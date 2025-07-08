namespace Crm.BusinessRules.CommunicationRules.EmailRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class TypeKeyRequired : RequiredRule<Email>
	{
		// Constructor
		public TypeKeyRequired()
		{
			Init(e => e.TypeKey);
		}
	}
}