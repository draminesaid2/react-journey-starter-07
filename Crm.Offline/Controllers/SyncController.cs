namespace Crm.Offline.Controllers
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;
	using System.Threading;
	using System.Threading.Tasks;

	using Autofac;

	using Crm.BackgroundServices;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Events;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Offline.Model;
	using Crm.Offline.Model.Request;

	using log4net;

	using LMobile.Unicore;

	using Main.Replication.Services;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using NHibernate;

	using Quartz;

	[Authorize]
	public class SyncController : Controller, ITransientDependency
	{
		private readonly ILog logger;
		private readonly SyncSegmentCache entityResultCache;
		private readonly IRepository<Posting> postingRepository;
		private readonly IModelProvider modelProvider;
		private readonly ILifetimeScope lifetimeScope;
		private readonly IScheduler scheduler;
		private readonly RestTypeProvider restTypeProvider;
		private readonly ISessionFactory sessionFactory;
		private readonly IUserService userService;
		private readonly Func<Posting> postingFactory;
		private readonly IAppSettingsProvider appSettingsProvider;
		public SyncController(IRepository<Posting> postingRepository, IModelProvider modelProvider, RestTypeProvider restTypeProvider, IScheduler scheduler, ILifetimeScope lifetimeScope, SyncSegmentCache entityResultCache, ILog logger, ISessionFactory sessionFactory, IUserService userService, Func<Posting> postingFactory, IAppSettingsProvider appSettingsProvider, IHttpContextAccessor httpContextAccessor)
		{
			this.postingRepository = postingRepository;
			this.modelProvider = modelProvider;
			this.restTypeProvider = restTypeProvider;
			this.scheduler = scheduler;
			this.lifetimeScope = lifetimeScope;
			this.entityResultCache = entityResultCache;
			this.logger = logger;
			this.sessionFactory = sessionFactory;
			this.userService = userService;
			this.postingFactory = postingFactory;
			this.appSettingsProvider = appSettingsProvider;
			this.httpContextAccessor = httpContextAccessor;
		}

		[HttpGet]
		public virtual EntityResult GetAll(string model, string pluginName, Guid? clientId = null, IDictionary<string, int?> replicationGroupSettings = null)
		{
			var type = modelProvider.GetType(model, pluginName);
			var idType = GetEntityIdType(type);
			var genericMethod = GetAllInfo.MakeGenericMethod(type, idType);
			return genericMethod.Invoke(this, new object[] { clientId, replicationGroupSettings }) as EntityResult;
		}
		private static readonly MethodInfo GetAllInfo = typeof(SyncController).GetMethod(nameof(GetAll), BindingFlags.NonPublic | BindingFlags.Instance);
		protected virtual EntityResult GetAll<TEntity, TId>(Guid? clientId, IDictionary<string, int?> replicationGroupSettings = null)
			where TEntity : class
			where TId : IComparable, IComparable<TId>, IEquatable<TId>
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			var syncDate = DateTime.UtcNow;
			var expirationDate = syncDate.AddHours(1);
			var syncService = GetSyncService<TEntity>();
			var replicationService = GetReplicationService<TEntity, TId>();
			clientId = replicationService.GetOrUpdateClientId(clientId, userService.CurrentUser);
			lifetimeScope.Resolve<IEnumerable<Library.Modularization.Events.IEventHandler<BeforeSyncEvent>>>().ForEach(x => x.Handle(new BeforeSyncEvent(userService.CurrentUser)));
			var entities = syncService.GetAll(userService.CurrentUser, replicationGroupSettings);
			var entityIds = entities == null ? new List<TId>() : replicationService.GetPendingEntityIds(entities, clientId.Value).ToList();
			var expiredIds = entities == null ? new List<TId>() : replicationService.GetExpiredEntityIds(entities, clientId.Value).ToList();

			logger.DebugFormat("GetAll<{0}>({1}), user: {2} fetched entity ids in {3} ms", typeof(TEntity).Name, clientId, userService.CurrentUser.Id, stopwatch.ElapsedMilliseconds);
			stopwatch.Restart();

			if (!entityIds.Any() && !expiredIds.Any())
			{
				return new EntityResult
				{
					ClientId = clientId.Value,
					EntityType = typeof(TEntity),
					Segment = 0,
					TotalSegments = 0
				};
			}

			var count = Math.Max(entityIds.Count, expiredIds.Count);
			var container = Guid.NewGuid();
			var segments = new List<EntityResult>();
			var totalSegments = Convert.ToInt32(Math.Ceiling(count / (decimal)syncService.SegmentSize));
			for (var i = 0; i < totalSegments; i++)
			{
				var entityResult = new EntityResult
				{
					ClientId = clientId.Value,
					Container = container,
					EntityType = typeof(TEntity),
					ExpirationDate = expirationDate,
					Segment = i + 1,
					Ids = entityIds.Skip(i * syncService.SegmentSize).Take(syncService.SegmentSize).Cast<object>().ToArray(),
					ExpiredIds = expiredIds.Skip(i * syncService.SegmentSize).Take(syncService.SegmentSize).Cast<object>().ToArray(),
					TotalSegments = totalSegments
				};
				segments.Add(entityResult);
			}

			logger.DebugFormat("GetAll<{0}>({1}), user: {2} prepared {3} segments in {4} ms", typeof(TEntity).Name, clientId, userService.CurrentUser.Id, totalSegments, stopwatch.ElapsedMilliseconds);
			stopwatch.Restart();
			entityResultCache.AddOrUpdate(container, segments.ToList(), (guid, results) => segments.ToList());
			var firstSegment = segments.Single(x => x.Segment == 1);
			PrefetchSegments<TEntity>(container, 1);
			firstSegment.FetchTask.Wait();
			logger.DebugFormat("GetAll<{0}>({1}), user: {2} prepared first segment in {3} ms", typeof(TEntity).Name, clientId, userService.CurrentUser.Id, stopwatch.ElapsedMilliseconds);
			stopwatch.Stop();
			return firstSegment;
		}
		protected virtual ISyncService<TEntity> GetSyncService<TEntity>()
			where TEntity : class
		{
			return lifetimeScope.Resolve<ISyncService<TEntity>>();
		}
		protected virtual IReplicationService<TEntity, TId> GetReplicationService<TEntity, TId>()
			where TEntity : class
			where TId : IComparable, IComparable<TId>, IEquatable<TId>
		{
			return lifetimeScope.Resolve<IReplicationService<TEntity, TId>>();
		}
		protected virtual void PrefetchSegments<TEntity>(Guid container, int segmentPrefetchAmount = 10)
			where TEntity : class
		{
			List<EntityResult> segments;
			if (entityResultCache.TryGetValue(container, out segments))
			{
				for (int i = 0; i < Math.Min(segments.Count, segmentPrefetchAmount); i++)
				{
					var segment = segments[i];
					if (segment.FetchTask == null || segment.FetchTask.IsFaulted || (segment.FetchTask.IsCompleted && segment.RestObjects == null))
					{
						var requestUrl = httpContextAccessor.HttpContext.Request.Path.ToString();
						var threadPrincipal = Thread.CurrentPrincipal;
						segment.FetchTask = Task.Factory.StartNew(
							() =>
							{
								Thread.CurrentPrincipal = threadPrincipal;
								//httpContextAccessor.HttpContext = new HttpContext(new HttpRequest(null, requestUrl, null), new HttpResponse(null));
								FetchSegmentRestEntities<TEntity>(segment);
							},
							CancellationToken.None,
							TaskCreationOptions.PreferFairness,
							TaskScheduler.Default);
					}
				}
			}
		}
		[HttpGet]
		public virtual EntityResult GetSegment(string model, string pluginName, [FromQuery] SegmentData segmentData)
		{
			if (!entityResultCache.ContainsKey(segmentData.Container))
			{
				throw new KeyNotFoundException(String.Format("Container {0} not found", segmentData.Container));
			}

			var type = modelProvider.GetType(model, pluginName);
			var idType = GetEntityIdType(type);
			var genericMethod = GetSegmentInfo.MakeGenericMethod(type, idType);
			return genericMethod.Invoke(this, new object[] { segmentData }) as EntityResult;
		}
		private static readonly MethodInfo GetSegmentInfo = typeof(SyncController).GetMethod(nameof(GetSegment), BindingFlags.NonPublic | BindingFlags.Instance);
		protected virtual EntityResult GetSegment<TEntity, TId>(SegmentData segmentData)
			where TEntity : class
			where TId : IComparable, IComparable<TId>, IEquatable<TId>
		{
			var container = segmentData.Container;
			var segment = segmentData.Segment;
			List<EntityResult> segments;
			if (entityResultCache.TryGetValue(container, out segments))
			{
				foreach (var previousSegment in segments.Where(x => x.Segment < segment).ToList())
				{
					PersistSegment<TEntity, TId>(previousSegment);
					segments.Remove(previousSegment);
				}

				foreach (EntityResult entityResult in segments)
				{
					entityResult.ExpirationDate = DateTime.UtcNow.AddHours(1);
				}

				var result = segments.SingleOrDefault(x => x.Segment == segment);
				if (result == null)
				{
					throw new KeyNotFoundException(String.Format("Segment {0} in container {1} not found", segment, container.ToString()));
				}

				while (result.FetchTask == null || !result.FetchTask.IsCompleted || result.RestObjects == null)
				{
					PrefetchSegments<TEntity>(container);
					result.FetchTask.Wait();
				}

				PrefetchSegments<TEntity>(container);
				return result;
			}

			throw new KeyNotFoundException(String.Format("EntityResults with Container Id {0} not found", container));
		}
		private static readonly MethodInfo PersistSegmentInfo = typeof(SyncController).GetMethod(nameof(PersistSegment), BindingFlags.NonPublic | BindingFlags.Instance);
		protected virtual void PersistSegment<TEntity, TId>(EntityResult segment)
			where TEntity : class
			where TId : IComparable, IComparable<TId>, IEquatable<TId>
		{
			var replicationService = GetReplicationService<TEntity, TId>();
			var expiredIds = segment.ExpiredIds?.Cast<TId>().ToArray();
			ICollection<(TId, DateTime)> modified = null;
			if (segment.Ids != null && segment.Ids.Any())
			{
				string idPropertyName;
				if (segment.EntityType.Implements<IEntityWithId>(true))
				{
					idPropertyName = nameof(IEntityWithId.Id);
				}
				else if (segment.EntityType.Implements<IIdentifiedObject>(true))
				{
					idPropertyName = nameof(IIdentifiedObject.UId);
				}
				else
				{
					throw new NotSupportedException($"Sync of entities with type {segment.EntityType.Name} is not supported, class should implement {nameof(IEntityWithId)} or {nameof(IIdentifiedObject)}");
				}
				var idProperty = segment.RestObjects.First().GetType().GetProperty(idPropertyName);
				if (idProperty == null)
				{
					throw new NotSupportedException($"Sync of entities with type {segment.EntityType.Name} is not supported, rest type {segment.RestObjects.First().GetType()} should have a {idPropertyName} property");
				}

				string modifyDatePropertyName;
				if (segment.EntityType.Implements<IAuditable>(true))
				{
					modifyDatePropertyName = nameof(IAuditable.ModifyDate);
				}
				else if (segment.EntityType.Implements<IAuditedObject>(true))
				{
					modifyDatePropertyName = nameof(IAuditedObject.ModifiedAt);
				}
				else
				{
					throw new NotSupportedException($"Sync of entities with type {segment.EntityType.Name} is not supported, class should implement {nameof(IAuditable)} or {nameof(IAuditedObject)}");
				}
				var modifyDateProperty = segment.RestObjects.First().GetType().GetProperty(modifyDatePropertyName);
				if (modifyDateProperty == null)
				{
					throw new NotSupportedException($"Sync of entities with type {segment.EntityType.Name} is not supported, rest type {segment.RestObjects.First().GetType()} should have a {modifyDatePropertyName} property");
				}
				modified = segment.RestObjects.Select(x => ((TId)idProperty.GetValue(x), (DateTime)modifyDateProperty.GetValue(x))).ToList();
			}
			if (modified?.Any() == true)
			{
				replicationService.PersistModifications(modified, segment.ClientId);
			}
			if (expiredIds?.Any() == true)
			{
				replicationService.PersistDeletionsById(expiredIds, segment.ClientId);
			}
		}
		protected virtual void FetchSegmentRestEntities<TEntity>(EntityResult segment)
			where TEntity : class
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			if (segment.RestObjects == null)
			{
				logger.DebugFormat("FetchSegmentRestEntities<{0}> fetched segment from entity result cache in {1} ms", typeof(TEntity).Name, stopwatch.ElapsedMilliseconds);
				stopwatch.Restart();
				using (var virtualRequestHandler = new VirtualRequestHandler(lifetimeScope))
				{
					var callingUser = virtualRequestHandler.GetLifetimeScope().Resolve<IUserService>().CurrentUser;
					virtualRequestHandler.GetLifetimeScope().Resolve<IEnumerable<Library.Modularization.Events.IEventHandler<BeforeSyncEvent>>>().ForEach(x => x.Handle(new BeforeSyncEvent(callingUser)));
					var syncService = virtualRequestHandler.GetLifetimeScope().Resolve<ISyncService<TEntity>>();
					var entities = syncService.Get(segment.Ids);
					entities = syncService.Eager(entities);
					var expands = syncService.Expands;

					var entityList = entities.ToList();
					logger.DebugFormat("FetchSegmentRestEntities<{0}> fetched entities from db in {1} ms", typeof(TEntity).Name, stopwatch.ElapsedMilliseconds);
					stopwatch.Restart();
					segment.RestObjects = ((IList)virtualRequestHandler.GetLifetimeScope().Resolve<RestTypeProvider>().CreateRestModel(entityList, options => options.Items["$expand"] = expands)).Cast<object>().ToArray();
					logger.DebugFormat("FetchSegmentRestEntities<{0}> created rest model in {1} ms", typeof(TEntity).Name, stopwatch.ElapsedMilliseconds);
					stopwatch.Stop();
				}
			}
		}
		[HttpGet]
		public virtual bool CompleteSegmentContainer(Guid container)
		{
			if (entityResultCache.TryGetValue(container, out var segments))
			{
				var entityType = segments.First().EntityType;
				var idType = GetEntityIdType(entityType);
				var genericMethod = PersistSegmentInfo.MakeGenericMethod(entityType, idType);
				foreach (var segment in segments)
				{
					genericMethod.Invoke(this, new object[] { segment });
				}
			}
			var result = entityResultCache.TryRemove(container, out segments);
			return result;
		}
		[HttpPost]
		public virtual bool Save(string transactionId = null)
		{
			CreatePosting(PostingType.Save, transactionId);
			return true;
		}
		[HttpDelete]
		public virtual bool Remove(string transactionId = null)
		{
			CreatePosting(PostingType.Remove, transactionId);
			return true;
		}
		protected virtual void CreatePosting(PostingType postingType, string transactionId = null)
		{
			var serializedEntity = Request.Body.GetStreamAsString();
			var posting = postingFactory();
			try
			{
				var entity = JsonConvert.DeserializeObject<IRestEntity>(serializedEntity);
				var entityId = GetEntityId(entity);
				if (String.IsNullOrEmpty(entityId) || entityId == default(int).ToString(CultureInfo.InvariantCulture) || entityId == default(Guid).ToString())
				{
					entityId = null;
				}
				posting.EntityTypeName = restTypeProvider.GetDomainType(entity.GetType()).Name;
				posting.EntityId = entityId;
			}
			catch (JsonSerializationException jsonSerializationException)
			{
				var deserializedEntity = JToken.Parse(serializedEntity);
				var assemblyQualifiedName = deserializedEntity.Value<string>("AssemblyQualifiedName");
				if (assemblyQualifiedName != null)
				{
					var restType = Type.GetType(assemblyQualifiedName);
					if (restType != null)
					{
						posting.EntityTypeName = restTypeProvider.GetDomainType(restType).Name;
					}
				}
				posting.EntityId = deserializedEntity.Value<string>("Id") ?? deserializedEntity.Value<string>("UId");
				logger.Error($"Failed deserializing {serializedEntity}", jsonSerializationException);
			}

			posting.SerializedEntity = serializedEntity;
			posting.PostingState = PostingState.Pending;
			posting.PostingType = postingType;
			posting.TransactionId = transactionId;
			postingRepository.SaveOrUpdate(posting);
		}
		protected virtual string GetEntityIdPropertyName(Type entityType)
		{
			var classMetadata = sessionFactory.GetClassMetadata(entityType);
			if (classMetadata != null)
			{
				return classMetadata.IdentifierPropertyName;
			}

			if (typeof(BusinessObject).IsAssignableFrom(entityType))
			{
				return nameof(BusinessObject.UId);
			}

			if (typeof(IEntityWithId).IsAssignableFrom(entityType))
			{
				return nameof(IEntityWithId.Id);
			}

			throw new NotSupportedException($"Failed to determine id property of {entityType.Name}");
		}
		protected virtual Type GetEntityIdType(Type entityType)
		{
			var idPropertyName = GetEntityIdPropertyName(entityType);
			var idType = entityType.GetPropertyDeclaredDeepestInHierarchy(idPropertyName).PropertyType;
			return idType;
		}
		protected virtual string GetEntityId(IRestEntity restEntity)
		{
			var restType = restEntity.GetType();
			var entityType = restTypeProvider.GetDomainType(restType);
			var idPropertyName = GetEntityIdPropertyName(entityType);
			var idProperty = restType.GetPropertyDeclaredDeepestInHierarchy(idPropertyName);
			return idProperty.GetValue(restEntity).ToString();
		}
		[HttpGet]
		public virtual IActionResult WaitForPostings()
		{
			var now = DateTime.UtcNow;
			var postingsProcessed = false;
			while (!postingsProcessed && DateTime.UtcNow - now < TimeSpan.FromSeconds(appSettingsProvider.GetValue(OfflinePlugin.Settings.WaitForPostingServiceTimeoutInSec)))
			{
				var jobDetails = scheduler.GetJobDetail(new JobKey(PostingService.JobName, PostingService.JobGroup));
				var previousFireTime = jobDetails.Result.JobDataMap.ContainsKey(PostingService.JobDataKeyPreviousFireTime) ? (DateTime?)jobDetails.Result.JobDataMap.GetDateTimeValueFromString(PostingService.JobDataKeyPreviousFireTime) : null;
				if (previousFireTime == null || previousFireTime.Value < now)
				{
					PostingService.Trigger(scheduler);
					Thread.Sleep(100);
					continue;
				}

				var previousFinishTime = jobDetails.Result.JobDataMap.ContainsKey(PostingService.JobDataKeyPreviousFinishTime) ? (DateTime?)jobDetails.Result.JobDataMap.GetDateTimeValueFromString(PostingService.JobDataKeyPreviousFinishTime) : null;
				if (previousFinishTime == null || previousFinishTime < now || previousFinishTime < previousFireTime)
				{
					Thread.Sleep(100);
				}
				else
				{
					postingsProcessed = true;
				}
			}

			return NoContent();
		}
		protected virtual Lazy<bool> EntityToSync<TEntity, TId>(Guid clientId, IDictionary<string, int?> replicationGroupSettings = null)
			where TEntity : class
			where TId : IComparable, IComparable<TId>, IEquatable<TId>
		{
			lifetimeScope.Resolve<IEnumerable<Library.Modularization.Events.IEventHandler<BeforeSyncEvent>>>().ForEach(x => x.Handle(new BeforeSyncEvent(userService.CurrentUser)));
			var syncService = GetSyncService<TEntity>();
			var replicationService = GetReplicationService<TEntity, TId>();
			var entities = syncService.GetAll(userService.CurrentUser, replicationGroupSettings);
			return entities == null ? new Lazy<bool>(false) : replicationService.AnyEntityToSync(entities, clientId);
		}
		private static readonly MethodInfo EntityToSyncInfo = typeof(SyncController).GetMethod(nameof(EntityToSync), BindingFlags.NonPublic | BindingFlags.Instance);
		private readonly IHttpContextAccessor httpContextAccessor;
		[HttpPut]
		public virtual IEnumerable<CheckOfflineRequestDefinition> Check([FromBody]CheckOfflineRequest checkOfflineRequest)
		{
			var clientIds = checkOfflineRequest.Definitions.Select(x => x.ClientId);
			var entitiesToSync = new Dictionary<CheckOfflineRequestDefinition, Lazy<bool>>();
			foreach (var definition in checkOfflineRequest.Definitions)
			{
				var type = modelProvider.GetType(definition.Model, definition.Plugin);
				var idType = GetEntityIdType(type);
				var genericMethod = EntityToSyncInfo.MakeGenericMethod(type, idType);
				entitiesToSync[definition] = (Lazy<bool>)genericMethod.Invoke(this, new object[] { definition.ClientId, checkOfflineRequest.ReplicationGroupSettings });
			}

			foreach (var item in entitiesToSync)
			{
				if (item.Value.Value)
				{
					yield return new CheckOfflineRequestDefinition()
					{
						Plugin = item.Key.Plugin,
						ClientId = item.Key.ClientId,
						Model = item.Key.Model,
						Index = item.Key.Index
					};
				}
			}
		}
	}
}
