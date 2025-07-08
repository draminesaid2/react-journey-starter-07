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

	public class TagSyncService : DefaultSyncService<Tag, Guid>
	{
		public TagSyncService(IRepositoryWithTypedId<Tag, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override IQueryable<Tag> GetAll(User user)
		{
			return repository.GetAll();
		}

		public override Type[] SyncDependencies => new[] { typeof(Contact) };

		public override Tag Save(Tag entity)
		{
			return repository.SaveOrUpdate(entity);
		}

		public override IQueryable<Tag> Eager(IQueryable<Tag> entities)
		{
			return entities;
		}
	}
}