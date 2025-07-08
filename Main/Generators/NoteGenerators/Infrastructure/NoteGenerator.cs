namespace Crm.Generators.NoteGenerators.Infrastructure
{
	using System;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Events;
	using Crm.Model;
	using Crm.Model.Notes;

	public abstract class NoteGenerator<T> : IEventHandler<T>
		where T : IEvent
	{
		private readonly IRepositoryWithTypedId<Note, Guid> noteRepository;

		public abstract Note GenerateNote(T e);
		public virtual void Handle(T e)
		{
			var note = GenerateNote(e);
			if (note != null)
			{
				note.AuthData = note.AuthData ?? (note.Contact?.AuthData != null ? new LMobile.Unicore.EntityAuthData { DomainId = note.Contact.AuthData.DomainId } : null);
				noteRepository.SaveOrUpdate(note);
				
				foreach (FileResource fileResource in note.Files)
				{
					fileResource.ParentId = note.Id;
					noteRepository.Session.Save(fileResource);
				}

				foreach (LinkResource linkResource in note.Links)
				{
					linkResource.ParentId = note.Id;
					noteRepository.Session.Save(linkResource);
				}
			}
		}

		protected NoteGenerator(IRepositoryWithTypedId<Note, Guid> noteRepository)
		{
			this.noteRepository = noteRepository;
		}
	}
}
