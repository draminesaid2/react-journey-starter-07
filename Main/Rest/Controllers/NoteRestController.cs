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
	using Crm.Model.Notes;

	public class NoteRestController : RestController<Note>
	{
		private readonly IRepositoryWithTypedId<Note, Guid> noteRepository;
		private readonly IRuleValidationService ruleValidationService;

		// Methods
		public virtual ActionResult Get(Guid id)
		{
			var note = noteRepository.Get(id);
			return Rest(note);
		}
		
		public virtual ActionResult Create(Note note)
		{
			var ruleViolations = ruleValidationService.GetRuleViolations(note);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			noteRepository.SaveOrUpdate(note);
			return Rest(note.Id.ToString());
		}
		
		public virtual ActionResult Update(Note note)
		{
			var ruleViolations = ruleValidationService.GetRuleViolations(note);
			if (ruleViolations.Any())
			{
				return Rest(ruleViolations);
			}

			noteRepository.SaveOrUpdate(note);
			return new EmptyResult();
		}
		
		[RequiredPermission(PermissionName.Delete, Group = MainPlugin.PermissionGroup.Note)]
		public virtual ActionResult Delete(Guid id)
		{
			var note = noteRepository.Get(id);
			noteRepository.Delete(note);
			return Content("Note deleted.");
		}

		public NoteRestController(IRepositoryWithTypedId<Note, Guid> noteRepository, IRuleValidationService ruleValidationService, RestTypeProvider restTypeProvider)
			: base(restTypeProvider)
		{
			this.noteRepository = noteRepository;
			this.ruleValidationService = ruleValidationService;
		}
	}
}
