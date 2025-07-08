namespace Crm.BackgroundServices
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Security.Principal;
	using System.Threading;

	using Autofac;

	using Crm.Events;
	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Extensions.IIdentity;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Modularization.Events;
	using Crm.Library.Services.Interfaces;

	using log4net;

	using Microsoft.Extensions.Hosting;

	using Quartz;

	[DisallowConcurrentExecution]
	[PersistJobDataAfterExecution]
	public class PostingService : JobBase
	{
		public const string JobDataKeyPreviousFinishTime = "PreviousFinishTime";
		public const string JobDataKeyPreviousFireTime = "PreviousFireTime";
		public const string JobGroup = "Core";
		public const string JobName = "PostingService";
		private Dictionary<string, Type> postingEntityTypes;
		private Dictionary<string, int> postingEntityTypePriorities;
		private readonly ILog logger;
		private readonly VirtualRequestHandler virtualRequestHandler;
		private readonly IUserService userService;
		private readonly IAppSettingsProvider appSettingsProvider;

		protected override JobFailureMode JobFailureMode
		{
			get { return JobFailureMode.Continue; }
		}

		protected override void Run(IJobExecutionContext context)
		{
			if (receivedShutdownSignal)
			{
				return;
			}
			context?.JobDetail.JobDataMap.PutAsString(JobDataKeyPreviousFireTime, DateTime.UtcNow);
			virtualRequestHandler.BeginRequest();
			var openPostings = virtualRequestHandler.GetLifetimeScope().Resolve<IRepository<Posting>>().GetAll()
				.Where(x => x.PostingState == PostingState.Pending || x.PostingState == PostingState.Failed)
				.ToList();
			UpdateStalePostings(openPostings);
			CheckForRelatedPostings(openPostings.Where(x => x.PostingState == PostingState.Pending || x.PostingState == PostingState.Failed).ToList());
			virtualRequestHandler.EndRequest();
			PostOpenPostings(openPostings.Where(x => x.PostingState == PostingState.Pending || x.PostingState == PostingState.Failed).ToList());
			context?.JobDetail.JobDataMap.PutAsString(JobDataKeyPreviousFinishTime, DateTime.UtcNow);
		}
		protected virtual void CheckForRelatedPostings(List<Posting> openPostings)
		{
			var postingRepository = virtualRequestHandler.GetLifetimeScope().Resolve<IRepository<Posting>>();
			foreach (var openPostingTransaction in openPostings.Where(x => x.TransactionId != null).GroupBy(x => x.TransactionId))
			{
				var unprocessedUserPostings = postingRepository.GetAll()
					.Where(x => x.PostingState == PostingState.Pending || x.PostingState == PostingState.Failed || x.PostingState == PostingState.Blocked)
					.Where(x => x.CreateUser == openPostingTransaction.First().CreateUser)
					.ToList();
				foreach (var openPosting in openPostingTransaction)
				{
					var entityType = postingEntityTypes[openPosting.EntityTypeName];
					var syncServiceType = typeof(ISyncService<>).MakeGenericType(entityType);
					var syncService = (ISyncService)virtualRequestHandler.GetLifetimeScope().Resolve(syncServiceType);
					var relatedUserPosting = unprocessedUserPostings.FirstOrDefault(x => x.TransactionId != openPostingTransaction.Key && syncService.IsRelated(openPosting.SerializedEntity, x.SerializedEntity));
					if (relatedUserPosting != null)
					{
						foreach (var posting in openPostingTransaction)
						{
							posting.PostingState = relatedUserPosting.PostingState == PostingState.Blocked ? PostingState.Blocked : posting.PostingState;
							posting.Retries = relatedUserPosting.Retries;
							posting.RetryAfter = relatedUserPosting.RetryAfter;
							posting.TransactionId = relatedUserPosting.TransactionId;
							postingRepository.SaveOrUpdate(posting);
						}

						break;
					}
				}
			}
		}
		protected virtual void PostOpenPostings(IList<Posting> openPostings)
		{
			var failedPostingEvents = new List<PostingFailedEvent>();

			var succesfulPostings = 0;
			var failedPostings = 0;

			foreach (var openPostingTransaction in openPostings.Where(x => x.TransactionId != null).GroupBy(x => x.TransactionId))
			{
				if (receivedShutdownSignal)
				{
					break;
				}
				try
				{
					if (openPostingTransaction.Any(x => x.Retries >= appSettingsProvider.GetValue(MainPlugin.Settings.Posting.MaxRetries)))
					{
						continue;
					}

					if (openPostingTransaction.Any(x => x.RetryAfter != null && x.RetryAfter > DateTime.UtcNow))
					{
						continue;
					}

					virtualRequestHandler.BeginRequest();
					foreach (var openEntityTypePostings in openPostingTransaction.GroupBy(x => new { x.EntityTypeName, Reverse = x.PostingType == PostingType.Remove }).OrderBy(x => postingEntityTypePriorities[x.Key.EntityTypeName] * (x.Key.Reverse ? -1 : 1)))
					{
						foreach (var openPosting in openEntityTypePostings.OrderBy(x => x.CreateDate).ThenBy(x => x.Id))
						{
							PostPosting(openPosting);
						}
					}
					virtualRequestHandler.EndRequest();
					succesfulPostings++;
				}
				catch (Exception ex)
				{
					// Clear session state and create new Session context for further processing
					logger.Error("An exception occured processing a posting transaction", ex);
					virtualRequestHandler.GetLifetimeScope().Resolve<ISessionProvider>().RollbackTransaction();
					virtualRequestHandler.EndRequest();
					virtualRequestHandler.BeginRequest();
					foreach (var openPosting in openPostingTransaction)
					{
						failedPostingEvents.Add(new PostingFailedEvent(openPosting, ex));
						failedPostings++;
					}
				}
				finally
				{
					virtualRequestHandler.EndRequest();
				}
			}

			foreach (Posting openPosting in openPostings.OrderBy(x => x.CreateDate).ThenBy(x => x.Id).Where(x => x.TransactionId == null))
			{
				if (receivedShutdownSignal)
				{
					break;
				}
				try
				{
					virtualRequestHandler.BeginRequest();
					PostPosting(openPosting);
					virtualRequestHandler.EndRequest();
					succesfulPostings++;
				}
				catch (Exception ex)
				{
					// Clear session state and create new Session context for further processing
					logger.Error("An exception occured processing a posting", ex);
					virtualRequestHandler.EndRequest();
					virtualRequestHandler.BeginRequest();
					failedPostingEvents.Add(new PostingFailedEvent(openPosting, ex));
					failedPostings++;
				}
				finally
				{
					virtualRequestHandler.EndRequest();
				}
			}

			virtualRequestHandler.BeginRequest();
			foreach (var failedPostingEvent in failedPostingEvents)
			{
				virtualRequestHandler.GetLifetimeScope().Resolve<IEventAggregator>().Publish(failedPostingEvent);
			}
			virtualRequestHandler.EndRequest();
		}

		protected virtual void PostPosting(Posting posting)
		{
			var entityType = postingEntityTypes[posting.EntityTypeName];
			var syncServiceType = typeof(ISyncService<>).MakeGenericType(entityType);
			var syncService = (ISyncService)virtualRequestHandler.GetLifetimeScope().Resolve(syncServiceType);

			var createUser = userService.GetUser(posting.CreateUser);
			Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(createUser.GetIdentityString()), new string[0]);

			if (posting.PostingType == PostingType.Save)
			{
				syncService.Save(posting.SerializedEntity);
			}
			else if (posting.PostingType == PostingType.Remove)
			{
				syncService.Remove(posting.SerializedEntity);
			}
			else
			{
				throw new NotImplementedException(String.Format("PostingType {0} unknown", posting.PostingType));
			}

			posting.PostingState = PostingState.Processed;
			posting.StateDetails = String.Empty;
			virtualRequestHandler.GetLifetimeScope().Resolve<IRepository<Posting>>().SaveOrUpdate(posting);
		}
		protected virtual void UpdateStalePostings(IList<Posting> openPostings)
		{
			var stalePostings = openPostings.Where(x => x.EntityId != null && openPostings.Any(y => y.EntityId == x.EntityId && y.EntityTypeName == x.EntityTypeName && y.PostingType == x.PostingType && y.CreateUser == x.CreateUser && y.CreateDate > x.CreateDate));
			foreach (Posting stalePosting in stalePostings)
			{
				if (receivedShutdownSignal)
				{
					break;
				}
				try
				{
					var successorPosting = openPostings.First(x => x.EntityId == stalePosting.EntityId && x.EntityTypeName == stalePosting.EntityTypeName && x.PostingType == stalePosting.PostingType && x.CreateUser == stalePosting.CreateUser && x.CreateDate > stalePosting.CreateDate);
					stalePosting.PostingState = PostingState.Stale;
					stalePosting.StateDetails = successorPosting.Id.ToString(CultureInfo.InvariantCulture);
					if (stalePosting.TransactionId != successorPosting.TransactionId)
					{
						foreach (var posting in openPostings.Where(x => x.TransactionId == stalePosting.TransactionId))
						{
							posting.TransactionId = successorPosting.TransactionId;
							virtualRequestHandler.GetLifetimeScope().Resolve<IRepository<Posting>>().SaveOrUpdate(posting);
						}
					}

					virtualRequestHandler.GetLifetimeScope().Resolve<IRepository<Posting>>().SaveOrUpdate(stalePosting);
				}
				catch (Exception ex)
				{
					logger.Error("An exception updating a stale posting", ex);
					virtualRequestHandler.GetLifetimeScope().Resolve<ISessionProvider>().RollbackTransaction();
					throw;
				}
			}
		}
		public static async void Trigger(IScheduler scheduler, double delay = 0)
		{
			if (!scheduler.IsStarted)
			{
				await scheduler.Start();
			}

			var nextTrigger = DateTime.Now.AddMilliseconds(delay);
			var alreadyTriggered = scheduler.GetTriggersOfJob(new JobKey(JobName, JobGroup)).Result.OfType<ISimpleTrigger>().Any(x => scheduler.GetTriggerState(x.Key).Result != TriggerState.Complete && x.GetNextFireTimeUtc() <= nextTrigger.AddMilliseconds(500) && x.GetNextFireTimeUtc() >= nextTrigger.AddMilliseconds(-500));
			if (alreadyTriggered)
			{
				return;
			}
			var trigger = TriggerBuilder
				.Create()
				.ForJob(JobName, JobGroup)
				.StartAt(nextTrigger)
				.Build();
				await scheduler.ScheduleJob(trigger);
			}

		public PostingService(
			VirtualRequestHandler virtualRequestHandler,
			IEnumerable<ISyncService> syncServices,
			ISessionProvider sessionProvider,
			ILog logger,
			IUserService userService,
			IAppSettingsProvider appSettingsProvider,
			IHostApplicationLifetime hostApplicationLifetime)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.virtualRequestHandler = virtualRequestHandler;
			this.logger = logger;
			this.userService = userService;
			this.appSettingsProvider = appSettingsProvider;
			InitializePostingEntityTypes(syncServices);
		}
		protected virtual Type GetSyncedType(ISyncService syncService)
		{
			return syncService.GetType().GetInterfaces().First(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(ISyncService<>)).GetGenericArguments().First();
		}
		protected virtual void InitializePostingEntityTypes(IEnumerable<ISyncService> syncServices)
		{
			var enumeratedSyncServices = syncServices as ISyncService[] ?? syncServices.ToArray();
			var syncServiceTypes = enumeratedSyncServices.Select(GetSyncedType).Distinct();
			postingEntityTypes = new Dictionary<string, Type>();
			foreach (Type syncServiceType in syncServiceTypes)
			{
				postingEntityTypes.Add(syncServiceType.Name, syncServiceType);
			}
			var postingEntityTypesByPriority = SortBySyncDependency(enumeratedSyncServices).Select(GetSyncedType).ToArray();
			postingEntityTypePriorities = postingEntityTypesByPriority.ToDictionary(x => x.Name, x => Array.IndexOf(postingEntityTypesByPriority, x));
		}

		protected virtual ISyncService[] SortBySyncDependency(IList<ISyncService> unsortedSyncServices)
		{
			var syncServices = unsortedSyncServices.ToList();
			var sortedSyncServices = new List<ISyncService>();
			while (syncServices.Any())
			{
				foreach (ISyncService syncService in syncServices.ToArray())
				{
					var entityTypeDependencies = syncService.SyncDependencies;
					var syncServiceDependencies = syncServices.Where(x => entityTypeDependencies.Any(y => y.IsAssignableFrom(GetSyncedType(x))));
					var hasDependencies = syncServiceDependencies.Any();
					if (!hasDependencies)
					{
						sortedSyncServices.Add(syncService);
						syncServices.Remove(syncService);
					}
				}
			}
			return sortedSyncServices.Where(unsortedSyncServices.Contains).ToArray();
		}
	}
}
