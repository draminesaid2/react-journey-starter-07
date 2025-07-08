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
	using Crm.Services.Interfaces;

	public class FileResourceSyncService : DefaultSyncService<FileResource, Guid>
	{
		private readonly IFileService fileService;
		public FileResourceSyncService(IRepositoryWithTypedId<FileResource, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IFileService fileService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.fileService = fileService;
		}
		public override Type[] SyncDependencies
		{
			get { return new[] { typeof(Note) }; }
		}
		public override IQueryable<FileResource> GetAll(User user)
		{
			return repository.GetAll();
		}

		public override FileResource Save(FileResource entity)
		{
			if (entity.Content == null || entity.Content.Length == 0)
			{
				entity.Content = repository.Get(entity.Id)?.Content;
			}
			fileService.SaveFile(entity);
			return entity;
		}

		public override IQueryable<FileResource> Eager(IQueryable<FileResource> entities)
		{
			return entities;
		}
	}
}