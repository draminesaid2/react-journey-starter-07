namespace Crm.BusinessRules.CommunicationRules.PhoneRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class DataRequired : RequiredRule<Phone>
	{
		// Constructor
		public DataRequired()
		{
			Init(p => p.Data, "Phone");
		}
	}
}