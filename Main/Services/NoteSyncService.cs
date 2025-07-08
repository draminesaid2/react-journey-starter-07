namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Model;
	using Crm.Model.Lookups;
	using Crm.Model.Notes;
	using Crm.Services.Interfaces;
	using NHibernate.Linq;

	public class NoteSyncService : DefaultSyncService<Note, Guid>
	{
		public static string NoteHistory = "NoteHistory";

		private readonly IPluginProvider pluginProvider;
		private readonly INoteService noteService;
		private readonly ILookupManager lookupManager;
		public NoteSyncService(IRepositoryWithTypedId<Note, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IPluginProvider pluginProvider, IMapper mapper, INoteService noteService, ILookupManager lookupManager)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.pluginProvider = pluginProvider;
			this.noteService = noteService;
			this.lookupManager = lookupManager;
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(Contact) }; }
		}
		public override IQueryable<Note> GetAll(User user, IDictionary<string, int?> groups)
		{
			var historySinceDays = groups == null ? 0 : ((groups.ContainsKey(NoteHistory) ? groups[NoteHistory] : null) ?? lookupManager.Get<ReplicationGroup>(NoteHistory)?.DefaultValue ?? 0);

			var syncableNoteTypes = lookupManager.List<NoteType>(x => x.IsSyncable).Select(x => x.Key).ToArray();
			var results = repository
				.GetAll()
				.Where(x => x.Plugin == null || pluginProvider.ActivePluginNames.Contains(x.Plugin));
			if (syncableNoteTypes.Any())
			{
				results = results.Where(x => syncableNoteTypes.Contains(x.NoteType));
			}

			results = noteService.Filter(results);

			if (historySinceDays > 0)
			{
				var historySince = DateTime.Now.AddDays(historySinceDays * -1);
				results = results.Where(x => x.CreateDate >= historySince);
			}

			return results;
		}

		public override IQueryable<Note> Eager(IQueryable<Note> entities)
		{
			return entities
				.Fetch(x => x.Contact)
				.FetchMany(x => x.Files)
				.FetchMany(x => x.Links);
		}
	}
}
