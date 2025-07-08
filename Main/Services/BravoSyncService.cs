namespace Crm.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Model;

	public class BravoSyncService : DefaultSyncService<Bravo, Guid>
	{
		public BravoSyncService(IRepositoryWithTypedId<Bravo, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override Bravo Save(Bravo entity)
		{
			return repository.SaveOrUpdate(entity);
		}
		public override IQueryable<Bravo> GetAll(User user)
		{
			return repository
				.GetAll()
				.Where(x => !x.IsOnlyVisibleForCreateUser || x.CreateUser == user.Id);
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(Contact) }; }
		}
	}
}