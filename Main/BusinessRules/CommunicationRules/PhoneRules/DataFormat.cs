namespace Crm.BusinessRules.CommunicationRules.PhoneRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class DataFormat : PhoneRule<Phone>
	{
		// Constructor
		public DataFormat()
		{
			Init(p => p.Data, "Phone");
		}
	}
}