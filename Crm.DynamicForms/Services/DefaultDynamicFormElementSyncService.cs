namespace Crm.DynamicForms.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.BaseModel;
	using Crm.Library.AutoFac;
	using Crm.Library.Data.Domain;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	public class DefaultDynamicFormElementSyncService<TEntity> : DefaultSyncService<TEntity, Guid>, IGenericDependency
		where TEntity : class, IDynamicFormElement, IEntityWithTypedId<Guid>
	{
		private readonly IRepositoryWithTypedId<DynamicFormResponse, Guid> dynamicFormResponseRepository;
		public DefaultDynamicFormElementSyncService(IRepositoryWithTypedId<TEntity, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IRepositoryWithTypedId<DynamicFormResponse, Guid> dynamicFormResponseRepository)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.dynamicFormResponseRepository = dynamicFormResponseRepository;
		}
		public override void Remove(TEntity entity)
		{
			var dynamicFormResponses = dynamicFormResponseRepository.GetAll().Where(x => x.DynamicFormElementKey == entity.Id);
			foreach (var dynamicFormResponse in dynamicFormResponses)
			{
				dynamicFormResponseRepository.Delete(dynamicFormResponse);
			}
			base.Remove(entity);
		}
	}
}