namespace Crm.DynamicForms.Services
{
	using System;
	using System.Linq;
	using System.Reflection;

	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.DynamicForms.Rest.Model;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	public class DynamicFormSyncService : DefaultSyncService<DynamicForm, Guid>
	{
		private readonly IAuthorizationManager authorizationManager;
		public DynamicFormSyncService(IRepositoryWithTypedId<DynamicForm, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IAuthorizationManager authorizationManager)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.authorizationManager = authorizationManager;
		}

		public override MemberInfo[] Expands => new[] { typeof(DynamicFormRest).GetMember(nameof(DynamicFormRest.Elements))[0] };
		
		public override IQueryable<DynamicForm> GetAll(User user)
		{
			var entities = repository.GetAll();
			if (!authorizationManager.IsAuthorizedForAction(user, DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Edit))
			{
				entities = entities.Where(x => x.Languages.Any(y => y.StatusKey == DynamicFormStatus.ReleasedKey));
			}
			return entities;
		}
	}
}
