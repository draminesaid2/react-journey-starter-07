namespace Crm.DynamicForms.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	using LinqKit;

	using NHibernate.Linq;

	public class DynamicFormResponseSyncService : DefaultSyncService<DynamicFormResponse, Guid>
	{
		private readonly IEnumerable<IDynamicFormReferenceSyncService> syncServices;

		public override Type[] SyncDependencies => new[] { typeof(DynamicFormReference) };

		public DynamicFormResponseSyncService(IRepositoryWithTypedId<DynamicFormResponse, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IEnumerable<IDynamicFormReferenceSyncService> syncServices)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.syncServices = syncServices;
		}

		public override IQueryable<DynamicFormResponse> GetAll(User user, IDictionary<string, int?> groups)
		{
			var entities = repository.GetAll();

			Expression<Func<DynamicFormResponse, bool>> predicate = null;
			foreach (var syncService in syncServices)
			{
				var dynamicFormReferenceIds = syncService.GetAllDynamicFormReferences(user, groups).Select(x => x.Id);
				predicate = predicate == null ? x => dynamicFormReferenceIds.Contains(x.DynamicFormReferenceKey) : predicate.Or(x => dynamicFormReferenceIds.Contains(x.DynamicFormReferenceKey));
			}

			return predicate == null ? entities.Where(x => false) : entities.Where(predicate);
		}

		public override IQueryable<DynamicFormResponse> Eager(IQueryable<DynamicFormResponse> entities)
		{
			return entities.Fetch(x => x.DynamicFormElement);
		}
	}
}
