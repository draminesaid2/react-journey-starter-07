using AutoMapper;
using Crm.Article.Model.Lookups;
using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Library.Rest;
using Crm.Library.Services;

namespace Crm.Article.Services
{
	using System.Linq;
	using Crm.Article.Model;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;

	public class ArticleDescriptionSyncService : DefaultLookupSyncService<ArticleDescription>
	{
		private readonly ISyncService<Article> articleSyncService;
		public ArticleDescriptionSyncService(IRepositoryWithTypedId<ArticleDescription, int> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISyncService<Article> articleSyncService) : base(repository, restTypeProvider, restSerializer, mapper) {
			this.articleSyncService = articleSyncService;
		}

		public override IQueryable<ArticleDescription> GetAll(User user)
		{
			var articles = articleSyncService.GetAll(user);
			return base.GetAll(user)
				.Where(x => articles.Any(y => y.ItemNo == x.Key));
		}
		public override ArticleDescription Save(ArticleDescription entity)
		{
			return repository.SaveOrUpdate(entity);
		}

		public override void Remove(ArticleDescription entity)
		{
			repository.Delete(entity);
		}
	}
}
