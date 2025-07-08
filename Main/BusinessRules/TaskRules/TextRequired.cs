namespace Crm.BusinessRules.TaskRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class TextRequired : RequiredRule<Task>
	{
		public TextRequired()
		{
			Init(t => t.Text);
		}
	}
}