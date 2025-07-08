using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using Crm.DynamicForms.Model;
using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Library.Model;
using Crm.Library.Rest;
using Crm.Library.Services;

using LinqKit;

namespace Crm.DynamicForms.Services
{
	public class DynamicFormFileResponseSyncService : DefaultSyncService<DynamicFormFileResponse, Guid>
	{
		private readonly IEnumerable<IDynamicFormReferenceSyncService> syncServices;

		public override Type[] SyncDependencies => new[] { typeof(DynamicFormReference) };
		public DynamicFormFileResponseSyncService(IRepositoryWithTypedId<DynamicFormFileResponse, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IEnumerable<IDynamicFormReferenceSyncService> syncServices)
				: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.syncServices = syncServices;
		}
		public override DynamicFormFileResponse Save(DynamicFormFileResponse entity)
		{
			return repository.SaveOrUpdate(entity);
		}

		public override void Remove(DynamicFormFileResponse entity)
		{
			repository.Delete(entity);
		}

		public override IQueryable<DynamicFormFileResponse> GetAll(User user, IDictionary<string, int?> groups)
		{
			var entities = repository.GetAll();

			var predicate = PredicateBuilder.New<DynamicFormFileResponse>(false);
			foreach (var syncService in syncServices)
			{
				var dynamicFormReferenceIds = syncService.GetAllDynamicFormReferences(user, groups).Select(x => x.Id);
				predicate = predicate.Or(x => dynamicFormReferenceIds.Contains(x.DynamicFormReferenceKey));
			}

			return entities.Where(predicate);
		}
	}
}
