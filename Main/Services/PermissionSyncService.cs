namespace Crm.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	using LMobile.Unicore;

	using User = Crm.Library.Model.User;

	public class PermissionSyncService : DefaultSyncService<Permission, Guid>
	{
		public PermissionSyncService(IRepositoryWithTypedId<Permission, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override IQueryable<Permission> GetAll(User user)
		{
			return repository.GetAll();
		}
		public override Permission Save(Permission entity)
		{
			throw new NotImplementedException();
		}
		public override void Remove(Permission entity)
		{
			throw new NotImplementedException();
		}
	}
}