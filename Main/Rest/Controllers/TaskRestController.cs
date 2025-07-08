using Microsoft.AspNetCore.Mvc;

namespace Crm.Rest.Controllers
{
	using System;
	using System.Linq;

	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.SearchCriteria;
	using Crm.Services.Interfaces;

	using PermissionGroup = MainPlugin.PermissionGroup;

	public class TaskRestController : RestController<Task>
	{
		private readonly ITaskService taskService;
		private readonly IRuleValidationService ruleValidationService;

		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.Task)]
		public virtual ActionResult Get(Guid id)
		{
			var task = taskService.GetTask(id);
			return Rest(task);
		}
		
		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.Task)]
		public virtual ActionResult Create(Task task)
		{
			var ruleViolations = ruleValidationService.GetRuleViolations(task);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			taskService.Save(task);
			return Content(task.Id.ToString());
		}
		
		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.Task)]
		public virtual ActionResult Update(Task task)
		{
			var ruleViolations = ruleValidationService.GetRuleViolations(task);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			taskService.Save(task);
			return new EmptyResult();
		}
		
		[RequiredPermission(MainPlugin.PermissionName.Complete, Group = PermissionGroup.Task)]
		public virtual ActionResult Complete(Guid id)
		{
			var task = taskService.GetTask(id);
			taskService.Complete(task);

			return new EmptyResult();
		}

		[RequiredPermission(PermissionName.Index, Group = PermissionGroup.Task)]
		public virtual ActionResult List(TaskSearchCriteria criteria)
		{
			var tasks = taskService.GetTasks(criteria).ToList();
			return Rest(tasks, "Tasks");
		}

		public TaskRestController(ITaskService taskService, IRuleValidationService ruleValidationService, RestTypeProvider restTypeProvider)
			: base(restTypeProvider)
		{
			this.taskService = taskService;
			this.ruleValidationService = ruleValidationService;
		}
	}
}
