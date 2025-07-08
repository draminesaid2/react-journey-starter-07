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

	public class DynamicFormElementRuleConditionSyncService : DefaultSyncService<DynamicFormElementRuleCondition, Guid>
	{
		private readonly ISyncService<DynamicFormElementRule> dynamicFormElementRuleSyncService;
		public DynamicFormElementRuleConditionSyncService(IRepositoryWithTypedId<DynamicFormElementRuleCondition, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<DynamicFormElementRule> dynamicFormElementRuleSyncService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.dynamicFormElementRuleSyncService = dynamicFormElementRuleSyncService;
		}
		public override IQueryable<DynamicFormElementRuleCondition> GetAll(User user, IDictionary<string, int?> groups)
		{
			var dynamicFormElementRules = dynamicFormElementRuleSyncService.GetAll(user, groups);
			return repository.GetAll()
				.Where(x => dynamicFormElementRules.Any(y => y.Id == x.DynamicFormElementRuleId));
		}
	}
}
