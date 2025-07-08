namespace Crm.Events
{
	using System.Collections.Generic;

	using Crm.Library.Modularization.Events;
	using System;
	public class NoteDeletedEvent : IEvent
	{
		public IList<Guid> NoteIds { get; protected set; }

		public NoteDeletedEvent(params Guid[] noteIds)
			: this(new List<Guid>(noteIds))
		{
		}

		public NoteDeletedEvent(IList<Guid> noteIds)
		{
			NoteIds = noteIds;
		}
	}
}