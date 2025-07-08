namespace Crm.Article.Services
{
	using System.Linq;
	using System.Reflection;

	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.OData.Query;

	using Microsoft.AspNetCore.Mvc;


	public class ODataQueryArticleDescriptionFilter : IODataQueryFunction, IDependency
	{
		private readonly IRepository<ArticleDescription> articleDescriptionRepository;
		private readonly IAuthorizationManager authorizationManager;
		private readonly IUserService userService;
		protected static MethodInfo FilterByArticleDescriptionInfo = typeof(ODataQueryArticleDescriptionFilter).GetMethod(nameof(FilterByArticleDescription), BindingFlags.Instance | BindingFlags.NonPublic);
		public ODataQueryArticleDescriptionFilter(IRepository<ArticleDescription> articleDescriptionRepository, IAuthorizationManager authorizationManager, IUserService userService)
		{
			this.articleDescriptionRepository = articleDescriptionRepository;
			this.authorizationManager = authorizationManager;
			this.userService = userService;
		}
		protected virtual IQueryable<T> FilterByArticleDescription<T>(IQueryable<T> query, string language, string filter) where T : Article
		{
			if (authorizationManager.IsAuthorizedForAction(userService.CurrentUser, PermissionGroup.WebApi, typeof(ArticleDescription).Name))
			{
				var subQuery = articleDescriptionRepository.GetAll()
					.Where(x => x.Language == language)
					.Where(x => x.Value.Contains(filter))
					.Select(x => x.Key);
				return query.Where(a => a.ItemNo.Contains(filter) || a.Description.Contains(filter) || subQuery.Contains(a.ItemNo));
			}
			return query.Where(a => a.ItemNo.Contains(filter) || a.Description.Contains(filter));
		}
		public virtual IQueryable<T> Apply<T, TRest>([FromQuery]ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(Article).IsAssignableFrom(typeof(T)) == false)
			{
				return query;
			}
			var language = options.Request.GetQueryParameter("filterByArticleDescriptionLanguage")?.Trim();
			var filter = options.Request.GetQueryParameter("filterByArticleDescriptionFilter")?.Trim();
			if (string.IsNullOrEmpty(language) == false && string.IsNullOrEmpty(filter) == false)
			{
				return (IQueryable<T>)FilterByArticleDescriptionInfo.MakeGenericMethod(typeof(T)).Invoke(this, new object[] { query, language, filter });
			}
			return query;
		}
	}
}
