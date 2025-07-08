namespace Crm.Events
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.Modularization.Events;

	public class PersonDeletedEvent : IEvent
	{
		public IList<Guid> PersonIds { get; protected set; }

		public PersonDeletedEvent(params Guid[] personIds)
			: this(new List<Guid>(personIds))
		{
		}

		public PersonDeletedEvent(IList<Guid> personIds)
		{
			PersonIds = personIds;
		}
	}
}