using System;
using System.Linq;

namespace Crm.Article.Services
{
	using AutoMapper;

	using Crm.Article.Model;
	using Crm.Article.Model.Relationships;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Model;
	public class ArticleCompanyRelationshipSyncService : DefaultSyncService<ArticleCompanyRelationship, Guid>
	{
		public ArticleCompanyRelationshipSyncService(IRepositoryWithTypedId<ArticleCompanyRelationship, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
		}
		public override Type[] SyncDependencies => new[] { typeof(Company), typeof(Article) };
		public override ArticleCompanyRelationship Save(ArticleCompanyRelationship entity)
		{
			return repository.SaveOrUpdate(entity);
		}
		public override IQueryable<ArticleCompanyRelationship> GetAll(User user)
		{
			return repository.GetAll();
		}
	}
}