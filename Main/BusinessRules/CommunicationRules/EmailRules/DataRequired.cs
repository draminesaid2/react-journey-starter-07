namespace Crm.BusinessRules.CommunicationRules.EmailRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class DataRequired : RequiredRule<Email>
	{
		// Constructor
		public DataRequired()
		{
			Init(e => e.Data, "Email");
		}
	}
}