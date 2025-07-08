namespace Crm.Services
{
	using System;
	using System.Linq;
	using System.Reflection;

	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Model;
	using Crm.Services.Interfaces;
	using Microsoft.AspNetCore.OData.Query;

	public class ODataQueryDocumentAttributeVisibilityFilter : IODataQueryFunction, IDependency
	{
		protected static MethodInfo FilterByVisibilityInfo = typeof(ODataQueryDocumentAttributeVisibilityFilter).GetMethod(nameof(FilterByContactVisibility), BindingFlags.Instance | BindingFlags.NonPublic);
		private readonly IDocumentAttributeVisibilityProvider visibilityProvider;
		private readonly IRepositoryWithTypedId<User, Guid> userRepository;

		public ODataQueryDocumentAttributeVisibilityFilter(IDocumentAttributeVisibilityProvider visibilityProvider, IRepositoryWithTypedId<User, Guid> userRepository)
		{
			this.visibilityProvider = visibilityProvider;
			this.userRepository = userRepository;
		}

		protected virtual IQueryable<DocumentAttribute> FilterByContactVisibility(IQueryable<DocumentAttribute> query, string username)
		{
			var user = userRepository.GetAll().Where(u => u.Id == username).SingleOrDefault();
			return visibilityProvider.FilterByContactVisibility(query, user).AsQueryable();
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(DocumentAttribute).IsAssignableFrom(typeof(T)) == false)
			{
				return query;
			}
			var user = options.Request.GetQueryParameter("filterDocumentAttributeByContactVisibility")?.Trim();
			if (string.IsNullOrEmpty(user) == false)
			{
				return (IQueryable<T>)FilterByVisibilityInfo.Invoke(this, new object[] { query, user });
			}
			return query;
		}
	}
}
