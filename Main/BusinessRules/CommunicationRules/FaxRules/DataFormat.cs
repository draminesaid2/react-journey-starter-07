namespace Crm.BusinessRules.CommunicationRules.FaxRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class DataFormat : FaxRule<Fax>
	{
		// Constructor
		public DataFormat()
		{
			Init(f => f.Data, "Fax");
		}
	}
}