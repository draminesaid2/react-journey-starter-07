namespace Crm.BusinessRules.TaskRules
{
	using System;

	using Crm.Library.Extensions;
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class ResponsibleRequired : RequiredRule<Task>
	{
		public ResponsibleRequired()
		{
			Init(t => t.ResponsibleUser);
		}
		public override bool IsSatisfiedBy(Task task)
		{
			return task.ResponsibleUser.IsNotNullOrEmpty() || (task.ResponsibleGroupKey != null && task.ResponsibleGroupKey != Guid.Empty);
		}
	}
}