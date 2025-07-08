namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Events;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Modularization.Events;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Model.Extensions;
	using Crm.SearchCriteria;
	using Crm.Services.Interfaces;

	using NHibernate.Linq;

	public class TaskService : ITaskService, IEventHandler<NoteDeletedEvent>
	{
		// Members
		private readonly IRepositoryWithTypedId<Task, Guid> taskRepository;
		private readonly IUserService userService;
		private readonly IEventAggregator eventAggregator;
		private readonly IRepositoryWithTypedId<Company, Guid> companyRepository;

		// Methods
		public virtual Task GetTask(Guid id)
		{
			return taskRepository.Get(id);
		}

		public virtual IEnumerable<Task> GetTasks(TaskSearchCriteria criteria)
		{
			var tasks = taskRepository.GetAll();
			if (criteria == null || !criteria.HasCriteria)
			{
				return new List<Task>().AsQueryable();
			}

			if (criteria.ContactId.HasValue)
			{
				var isCompany = companyRepository.Get(criteria.ContactId.Value) != null;
				if (isCompany)
				{
					tasks = tasks.Where(t => (t.ContactId != null && t.ContactId.Value == criteria.ContactId.Value) || (t.Contact != null && t.Contact.ParentId == criteria.ContactId.Value));
				}
				else
				{
					tasks = tasks.Where(t => t.ContactId.Value == criteria.ContactId.Value);
				}
			}

			var currentUser = userService.CurrentUser;
			if (criteria.FromCreateDate.HasValue)
			{
				tasks = tasks.Where(t => t.CreateDate >= criteria.FromCreateDate.Value);
			}

			if (criteria.ToCreateDate.HasValue)
			{
				tasks = tasks.Where(t => t.CreateDate <= criteria.ToCreateDate.Value);
			}

			if (criteria.ResponsibleUser.IsNotNullOrEmpty())
			{
				var user = userService.GetUser(criteria.ResponsibleUser);
				if (user != null)
				{
					var groups = user.Usergroups.Select(ug => ug.Id).ToList();
					tasks = tasks.Where(t => t.ResponsibleUser == criteria.ResponsibleUser || (t.ResponsibleGroupKey.HasValue && groups.Contains(t.ResponsibleGroupKey.Value)));
				}
			}
			else if (criteria.ResponsibleUsers != null)
			{
				var users = criteria.ResponsibleUsers;
				tasks = tasks.Where(t => users.Contains(t.ResponsibleUser));
			}

			if (criteria.NoteId.HasValue)
			{
				tasks = tasks.Where(t => t.NoteId.Value == criteria.NoteId.Value);
			}

			tasks = tasks.OrderBy(t => t.DueDate).ThenBy(t => t.DueTime).ThenBy(t => t.CreateDate).Fetch(x => x.Contact);
			return tasks.ToList();
		}

		public virtual IEnumerable<string> GetUsedTaskTypes()
		{
			return taskRepository.GetUsedTaskTypeKeys();
		}

		public virtual void Save(Task task)
		{
			var currentUser = userService.CurrentUser;
			Save(task, currentUser);
		}

		// ToDo: Can user parameter be replaced if current user is used?
		public virtual void Save(Task task, User user)
		{
			var isNewTask = task.Id.IsDefault();

			// Creator
			task.ModifyUser = user.Id;
			if (isNewTask)
			{
				task.CreateUser = user.Id;
			}
			if (task.ResponsibleGroupKey == null && task.ResponsibleGroup != null)
			{
				task.ResponsibleGroupKey = task.ResponsibleGroup.Id;
			}
			taskRepository.SaveOrUpdate(task);
		}

		public virtual void Complete(Task task)
		{
			if (task.IsCompleted)
			{
				return;
			}
			task.IsCompleted = true;
			taskRepository.SaveOrUpdate(task);
			eventAggregator.Publish(new TaskCompletedEvent(task));
		}

		public virtual void RemoveParentNote(Guid noteId)
		{
			taskRepository.RemoveParentNote(noteId);
		}

		public virtual void Handle(NoteDeletedEvent e)
		{
			foreach (var noteId in e.NoteIds)
			{
				RemoveParentNote(noteId);
			}
		}

		// Constructor
		protected TaskService(IEventAggregator eventAggregator, IRepositoryWithTypedId<Company, Guid> companyRepository)
		{
			this.eventAggregator = eventAggregator;
			this.companyRepository = companyRepository;
			// For Test Purposes only
		}
		public TaskService(IRepositoryWithTypedId<Task, Guid> taskRepository, IUserService userService, IEventAggregator eventAggregator, IRepositoryWithTypedId<Company, Guid> companyRepository)
		{
			this.taskRepository = taskRepository;
			this.userService = userService;
			this.eventAggregator = eventAggregator;
			this.companyRepository = companyRepository;
		}
	}
}
