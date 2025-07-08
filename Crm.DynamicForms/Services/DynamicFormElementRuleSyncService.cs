namespace Crm.DynamicForms.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;

	public class DynamicFormElementRuleSyncService : DefaultSyncService<DynamicFormElementRule, Guid>
	{
		private readonly ISyncService<DynamicForm> dynamicFormSyncService;
		public DynamicFormElementRuleSyncService(IRepositoryWithTypedId<DynamicFormElementRule, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<DynamicForm> dynamicFormSyncService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.dynamicFormSyncService = dynamicFormSyncService;
		}
		public override IQueryable<DynamicFormElementRule> GetAll(User user, IDictionary<string, int?> groups)
		{
			var dynamicForms = dynamicFormSyncService.GetAll(user, groups);
			return repository.GetAll()
				.Where(x => dynamicForms.Any(y => y.Id == x.DynamicFormId));
		}
	}
}
