namespace Crm.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;

	using NHibernate.Linq;

	public class CompanySyncService : DefaultSyncService<Company, Guid>
	{
		private readonly IVisibilityProvider visibilityProvider;
		public CompanySyncService(
			IRepositoryWithTypedId<Company, Guid> repository,
			RestTypeProvider restTypeProvider,
			IRestSerializer restSerializer,
			IMapper mapper,
			IVisibilityProvider visibilityProvider)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.visibilityProvider = visibilityProvider;
		}
		public override IQueryable<Company> GetAll(User user)
		{
			return visibilityProvider.FilterByVisibility(repository.GetAll());
		}
		public override IQueryable<Company> Eager(IQueryable<Company> entities)
		{
			return entities
				.Fetch(x => x.Parent);
		}
	}
}