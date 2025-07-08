namespace Crm.Controllers
{
	using System.Collections.Generic;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.EntityConfiguration;
	using Crm.Library.EntityConfiguration.Interfaces;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Model.Notes;

	using Microsoft.AspNetCore.Mvc;

	public class NoteListController : GenericListController<Note>
	{
		public NoteListController(IPluginProvider pluginProvider, IEnumerable<IRssFeedProvider<Note>> rssFeedProviders, IEnumerable<ICsvDefinition<Note>> csvDefinitions, IEntityConfigurationProvider<Note> entityConfigurationProvider, IRepository<Note> repository, IAppSettingsProvider appSettingsProvider, IResourceManager resourceManager, RestTypeProvider restTypeProvider)
			: base(pluginProvider, rssFeedProviders, csvDefinitions, entityConfigurationProvider, repository, appSettingsProvider, resourceManager, restTypeProvider)
		{
		}
		[RequiredPermission(PermissionName.Index, Group = MainPlugin.PermissionGroup.Note)]
		public override ActionResult FilterTemplate() => base.FilterTemplate();
		protected override string GetTitle()
		{
			return "Notes";
		}

		protected override string GetEmptySlate()
		{
			var param = Request.Query.Keys;

			return (param.Contains("DF_CreateDate") || param.Contains("DF_Text") || param.Contains("AC_User") || param.Contains("DF_NoteType"))
				? resourceManager.GetTranslation("NoRecordsForSearchCriteria")
				: resourceManager.GetTranslation("NoNoteInfo");
		}

		public virtual ActionResult TabContent()
		{
			var model = GetGenericListTemplateViewModel();
			model.EmptySlate = null;
			return PartialView(model);
		}

		[RequiredPermission(PermissionName.View, Group = MainPlugin.PermissionGroup.Note)]
		public override ActionResult IndexTemplate() => base.IndexTemplate();

		[RequiredPermission(PermissionName.Create, Group = MainPlugin.PermissionGroup.Note)]
		public override ActionResult MaterialPrimaryAction() => PartialView();
		public virtual ActionResult Attachments()
		{
			return PartialView("MaterialNoteAttachments");
		}
	}
}
