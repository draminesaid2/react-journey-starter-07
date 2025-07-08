namespace Crm.BusinessRules.StationRules
{
	using Crm.Library.Model;
	using Crm.Library.Validation.BaseRules;

	public class NameRequired : RequiredRule<Station>
	{
		// Constructor
		public NameRequired()
		{
			Init(s => s.Name);
		}
	}
}