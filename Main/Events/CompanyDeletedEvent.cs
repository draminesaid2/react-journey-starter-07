namespace Crm.Events
{
	using System;
	using System.Collections.Generic;

	using Crm.Library.Modularization.Events;

	public class CompanyDeletedEvent : IEvent
	{
		public IList<Guid> CompanyIds { get; protected set; }

		public CompanyDeletedEvent(params Guid[] companyIds)
			: this(new List<Guid>(companyIds))
		{
		}

		public CompanyDeletedEvent(IList<Guid> companyIds)
		{
			CompanyIds = companyIds;
		}
	}
}