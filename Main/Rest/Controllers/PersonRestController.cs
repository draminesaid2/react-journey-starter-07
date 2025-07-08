using Microsoft.AspNetCore.Mvc;

namespace Crm.Rest.Controllers
{
	using System;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation;
	using Crm.Model;
	using Crm.Services.Interfaces;

	using PermissionGroup = MainPlugin.PermissionGroup;

	public class PersonRestController : RestController<Person>
	{
		private readonly IPersonService personService;
		private readonly IUserService userService;
		private readonly ITagService tagService;
		private readonly IRepositoryWithTypedId<Task, Guid> taskRepository;
		private readonly IRuleValidationService ruleValidationService;

		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.Person)]
		public virtual ActionResult Get(Guid id)
		{
			var person = personService.GetPerson(id);
			return Rest(person);
		}
		
		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.Person)]
		public virtual ActionResult Create(Person person)
		{
			person.IsActive = true;
			var ruleViolations = ruleValidationService.GetRuleViolations(person);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			personService.SavePerson(person);
			return Rest(person.Id.ToString());
		}
		
		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.Person)]
		public virtual ActionResult Update(Person person)
		{
			var ruleViolations = ruleValidationService.GetRuleViolations(person);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			personService.SavePerson(person);
			return new EmptyResult();
		}
		
		[RequiredPermission(PermissionName.Delete, Group = PermissionGroup.Person)]
		public virtual ActionResult Delete(Guid id)
		{
			personService.DeletePerson(id);
			return Content("Person deleted.");
		}
		
		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.Person)]
		public virtual ActionResult ListAddresses(Guid id)
		{
			var person = personService.GetPerson(id);
			return Rest(person.Address, "Addresses");
		}
		
		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.Person)]
		public virtual ActionResult ListCommunications(Guid id, string commType)
		{
			var person = personService.GetPerson(id);

			switch (commType.ToLower())
			{
				case "phones":
					return Rest(person.Phones, "Phones");
				case "emails":
					return Rest(person.Emails, "Emails");
				case "faxes":
					return Rest(person.Faxes, "Faxes");
				case "websites":
					return Rest(person.Websites, "Websites");
				default:
					throw new ApplicationException("Invalid commType {0}. Check your route constraints.");
			}
		}
		
		[RequiredPermission(PermissionName.Create, Group = PermissionGroup.Person)]
		public virtual ActionResult CreateCommunication(Guid id, Communication communication)
		{
			var person = personService.GetPerson(id);
			person.Communications.Add(communication);
			personService.SavePerson(person);

			return new EmptyResult();
		}
		
		[RequiredPermission(PermissionName.Edit, Group = PermissionGroup.Person)]
		public virtual ActionResult UpdateCommunication(Guid id, Guid addressId, Communication communication)
		{
			var person = personService.GetPerson(id);

			if (!person.TryRemoveCommunication(communication.Id))
			{
				return Rest(new RuleViolation("PersonHasNoCommunicationWithThisId"));
			}

			communication.AddressId = person.Address.Id;
			communication.ContactId = person.Id;

			person.Communications.Add(communication);

			personService.SavePerson(person);

			return new EmptyResult();
		}
		
		[RequiredPermission(MainPlugin.PermissionName.DeleteCommunication, Group = PermissionGroup.Person)]
		public virtual ActionResult DeleteCommunication(Guid id, Guid commId, string commType)
		{
			var person = personService.GetPerson(id);

			if (!person.TryRemoveCommunication(commId))
			{
				return Rest(new RuleViolation("PersonHasNoCommunicationWithThisId"));
			}

			personService.SavePerson(person);

			return new EmptyResult();
		}
		
		[RequiredPermission(PermissionName.Read, Group = PermissionGroup.Person)]
		public virtual ActionResult ListTasks(Guid id)
		{
		    var tasks = taskRepository.GetAll().Where(x => x.ContactId == id && x.ResponsibleUser == userService.CurrentUser.Id);
            return Rest(tasks, "Tasks");
		}
		
		public virtual ActionResult ListTags(Guid id)
		{
			var tags = tagService.GetTagsByContactIds(new[] { id });
			return Rest(tags, "Tags", "Tag");
		}
		
		[RequiredPermission(MainPlugin.PermissionName.AssociateTag, Group = PermissionGroup.Person)]
		public virtual ActionResult AddTag(Guid id, string tagName)
		{
			tagService.AddTagToContact(id, tagName);
			return new EmptyResult();
		}
		
		[RequiredPermission(MainPlugin.PermissionName.RemoveTag, Group = PermissionGroup.Person)]
		public virtual ActionResult RemoveTag(Guid id, string tagName)
		{
			tagService.RemoveTagFromContact(id, tagName);
			return new EmptyResult();
		}

		public PersonRestController(IPersonService personService, IUserService userService, ITagService tagService, IRuleValidationService ruleValidationService, RestTypeProvider restTypeProvider, IRepositoryWithTypedId<Task, Guid> taskRepository)
			: base(restTypeProvider)
		{
			this.personService = personService;
			this.userService = userService;
			this.tagService = tagService;
			this.ruleValidationService = ruleValidationService;
			this.taskRepository = taskRepository;
		}
	}
}
