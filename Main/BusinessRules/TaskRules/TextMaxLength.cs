using Crm.Model;

namespace Crm.BusinessRules.TaskRules
{
	using Crm.Library.Validation.BaseRules;

	public class TextMaxLength : MaxLengthRule<Task>
	{
		// Constructor
		public TextMaxLength()
		{
			Init(t => t.Text, 4000);
		}
	}
}
