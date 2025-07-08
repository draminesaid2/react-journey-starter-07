namespace Crm.Controllers
{
	using System;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Modularization;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	[Authorize]
	public class ContactController : Controller
	{
		private readonly IRepositoryWithTypedId<Contact, Guid> contactRepository;
		private readonly IMergeService mergeService;
		private readonly IRecentPageService recentPageService;
		public ContactController(IRepositoryWithTypedId<Contact, Guid> contactRepository, IMergeService mergeService, IRecentPageService recentPageService)
		{
			this.contactRepository = contactRepository;
			this.mergeService = mergeService;
			this.recentPageService = recentPageService;
		}
		[RenderAction("CompanyListFilterTemplate")]
		[RenderAction("PersonListFilterTemplate")]
		public virtual ActionResult CommunicationsFilter() => PartialView();

		public virtual ActionResult RenameTagTemplate()
		{
			return PartialView();
		}

		public virtual ActionResult AddTag()
		{
			return PartialView();
		}

		[RequiredPermission("Merge", Group = "Contact")]
		public virtual ActionResult Merge(Guid loserId, Guid? winnerId)
		{
			if (!winnerId.HasValue)
			{
				return new NotFoundResult();
			}

			var loser = contactRepository.Get(loserId);
			var winner = contactRepository.Get(winnerId.Value);
			mergeService.Merge(loser.Self, winner.Self);
			recentPageService.RemoveRecentPagesByContactId(loser.Id);
			contactRepository.Delete(loser);
			return new EmptyResult();
		}

		[HttpGet]
		public virtual ActionResult Source(string source, string contactId)
		{
			return PartialView("SourceSelectBox");
		}
	}
}
