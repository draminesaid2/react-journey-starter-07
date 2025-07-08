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
	using Crm.Services.Interfaces;

	public class DocumentAttributeSyncService : DefaultSyncService<DocumentAttribute, Guid>
	{
		private readonly IDocumentAttributeVisibilityProvider documentAttributeVisibilityProvider;
		public DocumentAttributeSyncService(IRepositoryWithTypedId<DocumentAttribute, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IDocumentAttributeVisibilityProvider documentAttributeVisibilityProvider)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.documentAttributeVisibilityProvider = documentAttributeVisibilityProvider;
		}

		public override IQueryable<DocumentAttribute> GetAll(User user)
		{
			return documentAttributeVisibilityProvider.FilterByContactVisibility(repository.GetAll(), user);
		}

		public override IQueryable<DocumentAttribute> Eager(IQueryable<DocumentAttribute> entities)
		{
			return entities;
		}

		public override Type[] SyncDependencies => new[] { typeof(FileResource) };
	}
}
