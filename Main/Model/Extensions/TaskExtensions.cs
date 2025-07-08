namespace Crm.Model.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;

	public static class TaskExtensions
	{
		public static List<string> GetUsedTaskTypeKeys(this IRepositoryWithTypedId<Task, Guid> taskRepository)
		{
			return taskRepository.GetAll().Select(t => t.TypeKey).Distinct().ToList();
		}

		public static void RemoveParentNote(this IRepositoryWithTypedId<Task, Guid> taskRepository, Guid noteId)
		{
			foreach (Task task in taskRepository.GetAll().Where(t => t.NoteId == noteId))
			{
				task.NoteId = null;
				taskRepository.SaveOrUpdate(task);
			}
		}
	}
}