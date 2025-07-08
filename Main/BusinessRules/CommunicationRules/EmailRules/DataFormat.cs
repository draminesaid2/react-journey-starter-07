namespace Crm.BusinessRules.CommunicationRules.EmailRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class DataFormat : EmailRule<Email>
	{
		// Constructor
		public DataFormat()
		{
			Init(e => e.Data, "EMail");
		}
	}
}