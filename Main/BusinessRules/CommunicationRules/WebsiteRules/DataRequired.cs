namespace Crm.BusinessRules.CommunicationRules.WebsiteRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class DataRequired : RequiredRule<Website>
	{
		// Constructor
		public DataRequired()
		{
			Init(w => w.Data, "Website");
		}
	}
}