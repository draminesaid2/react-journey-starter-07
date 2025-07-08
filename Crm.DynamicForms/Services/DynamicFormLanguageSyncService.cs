namespace Crm.DynamicForms.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	public class DynamicFormLanguageSyncService : DefaultSyncService<DynamicFormLanguage, Guid>
	{
		public DynamicFormLanguageSyncService(IRepositoryWithTypedId<DynamicFormLanguage, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{}

		public override IQueryable<DynamicFormLanguage> GetAll(User user)
		{
			return repository.GetAll().Where(x => x.StatusKey == DynamicFormStatus.ReleasedKey);
		}
	}
}
