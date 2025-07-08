namespace Crm.AttributeForms.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Services;
	using Crm.AttributeForms.Model;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Model;

	public class AttributeFormSyncService : DefaultSyncService<AttributeForm, Guid>, IDynamicFormReferenceSyncService
	{
		public AttributeFormSyncService(IRepositoryWithTypedId<AttributeForm, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override Type[] SyncDependencies => new[] { typeof(Contact) };
		public override IQueryable<AttributeForm> GetAll(User user)
		{
			return repository.GetAll();
		}

		public virtual IQueryable<DynamicFormReference> GetAllDynamicFormReferences(User user, IDictionary<string, int?> groups)
		{
			return GetAll(user);
		}
	}
}
