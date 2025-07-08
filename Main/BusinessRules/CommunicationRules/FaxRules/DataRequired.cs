namespace Crm.BusinessRules.CommunicationRules.FaxRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class DataRequired : RequiredRule<Fax>
	{
		// Constructor
		public DataRequired()
		{
			Init(f => f.Data, "Fax");
		}
	}
}