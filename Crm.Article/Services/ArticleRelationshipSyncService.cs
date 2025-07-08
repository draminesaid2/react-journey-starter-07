namespace Crm.Article.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Article.Model.Relationships;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	public class ArticleRelationshipSyncService : DefaultSyncService<ArticleRelationship, Guid>
	{
		public ArticleRelationshipSyncService(IRepositoryWithTypedId<ArticleRelationship, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{}

		public override IQueryable<ArticleRelationship> GetAll(User user)
		{
			return repository.GetAll();
		}
		public override ArticleRelationship Save(ArticleRelationship entity)
		{
			return repository.SaveOrUpdate(entity);
		}
	}
}