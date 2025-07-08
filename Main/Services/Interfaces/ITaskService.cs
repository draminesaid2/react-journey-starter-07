namespace Crm.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using Crm.Library.AutoFac;
	using Crm.Library.Model;
	using Crm.Model;
	using Crm.SearchCriteria;

	public interface ITaskService : ITransientDependency
	{
		Task GetTask(Guid id);
		IEnumerable<Task> GetTasks(TaskSearchCriteria criteria);
		IEnumerable<string> GetUsedTaskTypes();
		void Save(Task task);
		void Save(Task task, User createUser);
		void Complete(Task task);
		void RemoveParentNote(Guid noteId);
	}
}
