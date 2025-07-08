namespace Crm.Offline.Model
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;

	using Crm.Library.AutoFac;
	using Crm.Library.Rest;

	public class SyncSegmentCache : ConcurrentDictionary<Guid, List<EntityResult>>, ISingletonDependency
	{
	}
}