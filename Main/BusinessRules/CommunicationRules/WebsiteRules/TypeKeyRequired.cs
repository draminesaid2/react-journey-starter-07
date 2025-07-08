namespace Crm.BusinessRules.CommunicationRules.WebsiteRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class TypeKeyRequired : RequiredRule<Website>
	{
		// Constructor
		public TypeKeyRequired()
		{
			Init(e => e.TypeKey);
		}
	}
}