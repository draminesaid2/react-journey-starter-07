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
	using Crm.Model.Notes;

	public class LinkResourceSyncService : DefaultSyncService<LinkResource, Guid>
	{
		public LinkResourceSyncService(IRepositoryWithTypedId<LinkResource, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(Note) }; }
		}
		public override IQueryable<LinkResource> GetAll(User user)
		{
			return repository.GetAll();
		}

		public override LinkResource Save(LinkResource entity)
		{
			return repository.SaveOrUpdate(entity);
		}

		public override IQueryable<LinkResource> Eager(IQueryable<LinkResource> entities)
		{
			return entities;
		}
	}
}