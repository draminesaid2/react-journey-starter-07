namespace Crm.DynamicForms.Services
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.DynamicForms.Model.Mappings;
	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Services.Interfaces;

	using Microsoft.AspNetCore.OData.Query;

	public class ODataQueryDynamicFormTitleFilter : IODataQueryFunction, IDependency
	{
		private readonly IUserService userService;
		private readonly IRepository<DynamicForm> dynamicFormRepository;
		private readonly IRepository<DynamicFormLocalization> dynamicFormLocalizationRepository;
		protected static MethodInfo FilterDynamicFormReferenceByTitleInfo = typeof(ODataQueryDynamicFormTitleFilter)
			.GetMethod(nameof(FilterDynamicFormReferenceByTitle), BindingFlags.Instance | BindingFlags.NonPublic);
		protected static MethodInfo FilterDynamicFormByTitleInfo = typeof(ODataQueryDynamicFormTitleFilter)
			.GetMethod(nameof(FilterDynamicFormByTitle), BindingFlags.Instance | BindingFlags.NonPublic);
		protected static MethodInfo OrderDynamicFormReferenceByTitleInfo = typeof(ODataQueryDynamicFormTitleFilter)
			.GetMethod(nameof(OrderDynamicFormReferenceByTitle), BindingFlags.Instance | BindingFlags.NonPublic);
		protected static MethodInfo OrderDynamicFormByTitleInfo = typeof(ODataQueryDynamicFormTitleFilter)
			.GetMethod(nameof(OrderDynamicFormByTitle), BindingFlags.Instance | BindingFlags.NonPublic);
		public ODataQueryDynamicFormTitleFilter(IUserService userService, IRepository<DynamicForm> dynamicFormRepository, IRepository<DynamicFormLocalization> dynamicFormLocalizationRepository)
		{
			this.userService = userService;
			this.dynamicFormRepository = dynamicFormRepository;
			this.dynamicFormLocalizationRepository = dynamicFormLocalizationRepository;
		}
		protected virtual IQueryable<T> FilterDynamicFormReferenceByTitle<T>(IQueryable<T> query, string filter, string statusKey) where T : DynamicFormReference
		{
			var languageKey = userService.CurrentUser.DefaultLanguageKey;
			dynamicFormRepository.Session.EnableFilter(DynamicFormLocalizationFilter.Name);
			Expression<Func<T, bool>> filterByLocalizations = x => x.DynamicForm.Localizations
				.Where(l => l.DynamicFormElementId == null)
				.Where(l => l.Language == x.DynamicForm.DefaultLanguageKey || l.Language == languageKey)
				.Any(l => string.IsNullOrEmpty(filter) || l.Value.Contains(filter));
			return query
				.Where(x => string.IsNullOrEmpty(statusKey) || x.DynamicForm.Languages.Any(l => l.StatusKey == DynamicFormStatus.ReleasedKey))
				.Where(filterByLocalizations);
		}
		protected virtual IQueryable<DynamicForm> FilterDynamicFormByTitle(IQueryable<DynamicForm> query, string filter, string statusKey)
		{
			var languageKey = userService.CurrentUser.DefaultLanguageKey;
			dynamicFormRepository.Session.EnableFilter(DynamicFormLocalizationFilter.Name);
			Expression<Func<DynamicForm, bool>> filterByLocalizations = x => x.Localizations
				.Where(l => l.DynamicFormElementId == null)
				.Where(l => l.Language == x.DefaultLanguageKey || l.Language == languageKey)
				.Any(l => string.IsNullOrEmpty(filter) || l.Value.Contains(filter));
			return query
				.Where(x => string.IsNullOrEmpty(statusKey) || x.Languages.Any(l => l.StatusKey == statusKey))
				.Where(filterByLocalizations);
		}
		protected virtual IQueryable<T> OrderDynamicFormReferenceByTitle<T>(IQueryable<T> query, string language, string direction)
			where T : DynamicFormReference
		{
			Expression<Func<T, IQueryable<string>>> orderByExpression = x => dynamicFormLocalizationRepository.GetAll().Where(y => y.DynamicFormId == x.DynamicFormKey && y.DynamicFormElementId == null && ((x.DynamicForm.Languages.Any(z => z.LanguageKey == language) && y.Language == language) || (!x.DynamicForm.Languages.Any(z => z.LanguageKey == language) && y.Language == x.DynamicForm.DefaultLanguageKey))).Take(1).Select(y => y.Value);
			var orderAscending = direction.Equals("ASC", StringComparison.InvariantCultureIgnoreCase);
			return orderAscending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);
		}
		protected virtual IQueryable<DynamicForm> OrderDynamicFormByTitle(IQueryable<DynamicForm> query, string language, string direction)
		{
			Expression<Func<DynamicForm, IQueryable<string>>> orderByExpression = x => dynamicFormLocalizationRepository.GetAll().Where(y => y.DynamicFormId == x.Id && y.DynamicFormElementId == null && ((x.Languages.Any(z => z.LanguageKey == language) && y.Language == language) || (!x.Languages.Any(z => z.LanguageKey == language) && y.Language == x.DefaultLanguageKey))).Take(1).Select(y => y.Value);
			var orderAscending = direction.Equals("ASC", StringComparison.InvariantCultureIgnoreCase);
			return orderAscending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			MethodInfo filterBy;
			MethodInfo orderBy;
			if (typeof(DynamicFormReference).IsAssignableFrom(typeof(T)))
			{
				filterBy = FilterDynamicFormReferenceByTitleInfo.MakeGenericMethod(typeof(T));
				orderBy = OrderDynamicFormReferenceByTitleInfo.MakeGenericMethod(typeof(T));
			}
			else if (typeof(DynamicForm).IsAssignableFrom(typeof(T)))
			{
				filterBy = FilterDynamicFormByTitleInfo;
				orderBy = OrderDynamicFormByTitleInfo;
			}
			else
			{
				return query;
			}
			const string parameterName = "filterByDynamicFormTitle";
			var parameters = options.Request.Query;
			if (parameters.Keys.Contains(parameterName))
			{
				var filter = options.Request.GetQueryParameter("filterByDynamicFormTitle")?.Trim();
				var statusKey = options.Request.GetQueryParameter("filterByDynamicFormStatusKey")?.Trim();
				query = (IQueryable<T>)filterBy.Invoke(this, new object[] { query, filter, statusKey });
			}

			const string orderByLanguageParameterName = "orderByDynamicFormTitleLanguage";
			const string orderByDirectionParameterName = "orderByDynamicFormTitleDirection";
			if (parameters.Keys.Contains(orderByLanguageParameterName) && parameters.Keys.Contains(orderByDirectionParameterName))
			{
				var language = options.Request.GetQueryParameter(orderByLanguageParameterName)?.Trim();
				var direction = options.Request.GetQueryParameter(orderByDirectionParameterName)?.Trim();
				query = (IQueryable<T>)orderBy.Invoke(this, new object[] { query, language, direction });
			}

			return query;
		}
	}
}
