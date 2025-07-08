namespace Crm.Offline.BackgroundServices
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Rest;
	using Crm.Offline.Model;

	using log4net;

	using Microsoft.Extensions.Hosting;

	using Quartz;

	[DisallowConcurrentExecution]
	public class SyncSegmentCleanupAgent : JobBase
	{
		private readonly SyncSegmentCache syncSegmentCache;
		protected override JobFailureMode JobFailureMode
		{
			get { return JobFailureMode.Continue; }
		}
		protected override void Run(IJobExecutionContext context)
		{
			foreach (Guid container in syncSegmentCache.Keys)
			{
				if (receivedShutdownSignal)
				{
					break;
				}

				List<EntityResult> entityResults;
				if (syncSegmentCache.TryGetValue(container, out entityResults))
				{
					if (entityResults.First().ExpirationDate < DateTime.UtcNow.AddHours(-1))
					{
						List<EntityResult> expired;
						syncSegmentCache.TryRemove(container, out expired);
					}
				}
			}
		}
		public SyncSegmentCleanupAgent(ISessionProvider sessionProvider, SyncSegmentCache syncSegmentCache, ILog logger, IHostApplicationLifetime hostApplicationLifetime)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.syncSegmentCache = syncSegmentCache;
		}
	}
}