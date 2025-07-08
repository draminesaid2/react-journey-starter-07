namespace Crm.Generators.NoteGenerators
{
	using System;

	using Crm.Events;
	using Crm.Generators.NoteGenerators.Infrastructure;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Model.Notes;

	public class TaskCompletedNoteGenerator : NoteGenerator<TaskCompletedEvent>
	{
		private readonly IPluginProvider pluginProvider;
		private readonly Func<TaskCompletedNote> noteFactory;
		public override Note GenerateNote(TaskCompletedEvent e)
		{
			var task = e.Task;
			var contact = task.Contact;

			Guid? contactId = null;
			var plugin = "Main";
			if (contact != null)
			{
				contactId = contact.Id;
				var pluginDescriptor = pluginProvider.FindPluginDescriptor(contact.ActualType);
				plugin = pluginDescriptor.PluginName;
			}

			var note = noteFactory();
			note.IsActive = true;
			note.ContactId = contactId;
			note.AuthData = contact?.AuthData != null ? new LMobile.Unicore.EntityAuthData { DomainId = contact.AuthData.DomainId } : null;
			note.Text = task.Text;
			note.Plugin = plugin;

			return note;
		}
		public TaskCompletedNoteGenerator(IPluginProvider pluginProvider, IRepositoryWithTypedId<Note, Guid> noteRepository, Func<TaskCompletedNote> noteFactory)
			: base(noteRepository)
		{
			this.pluginProvider = pluginProvider;
			this.noteFactory = noteFactory;
		}
	}
}
