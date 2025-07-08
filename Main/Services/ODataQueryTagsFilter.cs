namespace Crm.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Model;

	using Microsoft.AspNetCore.OData.Query;

	public class ODataQueryTagsFilter : IODataQueryFunction, IDependency
	{
		protected static MethodInfo FilterByTagsInfo = typeof(ODataQueryTagsFilter).GetMethod(nameof(FilterByTags), BindingFlags.Instance | BindingFlags.NonPublic);
		protected virtual IQueryable<T> FilterByTags<T>(IQueryable<T> query, List<string> tags) where T : Contact
		{
			return query.Where(c => c.Tags.Count(t => tags.Contains(t.Name)) == tags.Count);
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(Contact).IsAssignableFrom(typeof(T)) == false)
			{
				return query;
			}
			var parameters = options.Request.Query;
			var tags = parameters.Keys.Where(x => x.StartsWith("filterByTags")).SelectMany(x => parameters[x].ToArray()).ToList();
			if (tags.Any())
			{
				return (IQueryable<T>)FilterByTagsInfo.MakeGenericMethod(typeof(T)).Invoke(this, new object[] { query, tags });
			}
			return query;
		}
	}
}
