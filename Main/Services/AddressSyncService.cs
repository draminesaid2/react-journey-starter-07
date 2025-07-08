namespace Crm.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Model;
	using Crm.Model.Extensions;

	using NHibernate.Linq;

	public class AddressSyncService : DefaultSyncService<Address, Guid>
	{
		private readonly IAuthorizationManager authorizationManager;
		public AddressSyncService(IRepositoryWithTypedId<Address, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IAuthorizationManager authorizationManager)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.authorizationManager = authorizationManager;
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(Company) }; }
		}
		public override IQueryable<Address> GetAll(User user)
		{
			return repository
				.GetAll()
				.FilterByContactVisibility(authorizationManager, user);
		}
		public override IQueryable<Address> Eager(IQueryable<Address> entities)
		{
			return entities
				.Fetch(x => x.Contact);
		}
	}
}