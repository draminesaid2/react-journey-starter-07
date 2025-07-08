namespace Crm.BusinessRules.StationRules
{
	using Crm.Library.Model;
	using Crm.Library.Validation.BaseRules;

	public class NameMaxLength : MaxLengthRule<Station>
	{
		// Constructor
		public NameMaxLength()
		{
			Init(s => s.Name, 150);
		}
	}
}