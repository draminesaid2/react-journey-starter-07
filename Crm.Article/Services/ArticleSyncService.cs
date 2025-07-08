namespace Crm.Article.Services
{
	using System;
	using System.Linq;

	using AutoMapper;

	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;

	public class ArticleSyncService : DefaultSyncService<Article, Guid>
	{
		private readonly IVisibilityProvider visibilityProvider;
		private static readonly string[] SyncedArticleTypes = { ArticleType.CostKey, ArticleType.MaterialKey, ArticleType.ServiceKey, ArticleType.ToolKey };

		public ArticleSyncService(
			IRepositoryWithTypedId<Article, Guid> repository,
			RestTypeProvider restTypeProvider,
			IRestSerializer restSerializer,
			IMapper mapper,
			IVisibilityProvider visibilityProvider)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.visibilityProvider = visibilityProvider;
		}
		public override IQueryable<Article> GetAll(User user)
		{
			var query = repository.GetAll().Where(x => SyncedArticleTypes.Contains(x.ArticleTypeKey));
			return visibilityProvider.FilterByVisibility(query);
		}
	}
}
