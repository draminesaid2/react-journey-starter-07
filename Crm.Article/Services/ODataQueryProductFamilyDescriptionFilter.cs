using System.Linq;

namespace Crm.Article.Services
{
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

	public class ODataQueryProductFamilyDescriptionFilter : IODataQueryFunction, IDependency
	{
		private readonly IRepository<ProductFamilyDescription> productfamilyDescriptionRepository;
		private readonly IAuthorizationManager authorizationManager;
		private readonly IUserService userService;
		protected static MethodInfo FilterByProductFamilyDescriptionInfo = typeof(ODataQueryProductFamilyDescriptionFilter).GetMethod(nameof(FilterByProductFamilyDescription), BindingFlags.Instance | BindingFlags.NonPublic);
		public ODataQueryProductFamilyDescriptionFilter(IRepository<ProductFamilyDescription> productfamilyDescriptionRepository, IAuthorizationManager authorizationManager, IUserService userService)
		{
			this.productfamilyDescriptionRepository = productfamilyDescriptionRepository;
			this.authorizationManager = authorizationManager;
			this.userService = userService;
		}
		protected virtual IQueryable<T> FilterByProductFamilyDescription<T>(IQueryable<T> query, string language, string filter) where T : ProductFamily
		{
			if (authorizationManager.IsAuthorizedForAction(userService.CurrentUser, PermissionGroup.WebApi, typeof(ProductFamilyDescription).Name))
			{
				var subQuery = productfamilyDescriptionRepository.GetAll()
					.Where(x => x.Language == language)
					.Where(x => x.Value.Contains(filter))
					.Select(x => x.Key);
				return query.Where(a => a.Name.Contains(filter) || subQuery.Contains(a.Id.ToString()));
			}
			return query.Where(a => a.Name.Contains(filter));
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(ProductFamily).IsAssignableFrom(typeof(T)) == false)
			{
				return query;
			}
			var language = options.Request.GetQueryParameter("filterByProductFamilyDescriptionLanguage")?.Trim();
			var filter = options.Request.GetQueryParameter("filterByProductFamilyDescriptionFilter")?.Trim();
			if (string.IsNullOrEmpty(language) == false && string.IsNullOrEmpty(filter) == false)
			{
				return (IQueryable<T>)FilterByProductFamilyDescriptionInfo.MakeGenericMethod(typeof(T)).Invoke(this, new object[] { query, language, filter });
			}
			return query;
		}
	}
}
