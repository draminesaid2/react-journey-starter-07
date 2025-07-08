namespace Crm.Article.BackgroundServices
{
	using System;
	using System.Linq;

	using Crm.Article.Model;
	using Crm.Library.BackgroundServices;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;

	using log4net;

	using Microsoft.Extensions.Hosting;

	using Quartz;

	[DisallowConcurrentExecution]
	public class ArticleExpirationCheckerAgent : JobBase
	{
		private readonly IRepository<Article> articleRepository;

		protected override void Run(IJobExecutionContext context)
		{
			CheckHasExpired();
		}

		protected virtual void CheckHasExpired()
		{
			var today = DateTime.UtcNow.Date;
			var disableArticles = articleRepository.GetAll().Where(x => x.ValidTo.HasValue && x.IsEnabled == true && x.ValidTo.Value < today);
			foreach (var article in disableArticles)
			{
				article.IsEnabled = false;
				articleRepository.SaveOrUpdate(article);
			}

			var enableArticles = articleRepository.GetAll()
				.Where(x => x.ValidFrom.HasValue && x.IsEnabled == false && x.ValidFrom.Value <= today && (!x.ValidTo.HasValue || x.ValidTo.Value > today));
			foreach (var article in enableArticles)
			{
				article.IsEnabled = true;
				articleRepository.SaveOrUpdate(article);
			}
		}

		public ArticleExpirationCheckerAgent(ISessionProvider sessionProvider, IRepository<Article> articleRepository, ILog logger, IHostApplicationLifetime hostApplicationLifetime)
			: base(sessionProvider, logger, hostApplicationLifetime)
		{
			this.articleRepository = articleRepository;
		}
	}
}
