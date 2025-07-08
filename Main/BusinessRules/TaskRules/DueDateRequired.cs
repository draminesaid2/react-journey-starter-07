namespace Crm.BusinessRules.TaskRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class DueDateRequired : RequiredRule<Task>
	{
		public DueDateRequired()
		{
			Init(t => t.DueDate);
		}
	}
}