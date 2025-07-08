namespace Crm.AttributeForms.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Crm.DynamicForms.Model.BaseModel;
	using Crm.AttributeForms.Model;
	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Model;

	using Microsoft.AspNetCore.OData.Query;

	public class ODataQueryAttributeFormPropertyFilter : IODataQueryFunction, IDependency
	{
		private readonly IRepositoryWithTypedId<AttributeForm, Guid> attributeFormRepository;
		private readonly IRepositoryWithTypedId<DynamicFormElement, Guid> dynamicFormElementRepository;
		protected static MethodInfo FilterByAttributeFormPropertyInfo = typeof(ODataQueryAttributeFormPropertyFilter)
			.GetMethod(nameof(FilterByAttributeFormProperty), BindingFlags.Instance | BindingFlags.NonPublic);
		public ODataQueryAttributeFormPropertyFilter(IRepositoryWithTypedId<AttributeForm, Guid> attributeFormRepository, IRepositoryWithTypedId<DynamicFormElement, Guid> dynamicFormElementRepository)
		{
			this.attributeFormRepository = attributeFormRepository;
			this.dynamicFormElementRepository = dynamicFormElementRepository;
		}
		protected virtual IQueryable<T> FilterByAttributeFormProperty<T>(IQueryable<T> query, Dictionary<Guid, string> filters) where T : Contact
		{
			foreach (Guid dynamicFormElementKey in filters.Keys)
			{
				var dynamicFormElementType = dynamicFormElementRepository.Get(dynamicFormElementKey).FormElementType;
				var contactIds = attributeFormRepository.GetAll()
					.Where(x => !x.Completed);

				if (dynamicFormElementType == "SingleLineText" || dynamicFormElementType == "MultiLineText")
				{
					contactIds = contactIds.Where(x => x.Responses.Any(response => response.DynamicFormElementKey == dynamicFormElementKey && response.ValueAsString.Contains(filters[dynamicFormElementKey])));
				}
				else if (dynamicFormElementType == "Date")
				{
					var startDay = DateTime.Parse(filters[dynamicFormElementKey], null, System.Globalization.DateTimeStyles.RoundtripKind);
					var endDay = DateTime.Parse(filters[dynamicFormElementKey], null, System.Globalization.DateTimeStyles.RoundtripKind).AddTicks(-1).AddDays(1);
					contactIds = contactIds.Where(x => x.Responses.Any(response => response.DynamicFormElementKey == dynamicFormElementKey && DateTime.Parse(response.ValueAsString) >= startDay && DateTime.Parse(response.ValueAsString) <= endDay));
				}
				else
				{
					contactIds = contactIds.Where(x => x.Responses.Any(response => response.DynamicFormElementKey == dynamicFormElementKey && response.ValueAsString == filters[dynamicFormElementKey]));
				}


				query = query.Where(x => contactIds.Select(y => y.ReferenceKey.Value).Contains(x.Id));
			}
			return query;
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
			var filters = parameters.Keys.Where(x => x.StartsWith("filterByAttributeFormProperty"))
				.ToDictionary(x => Guid.Parse(x.Split('_')[1]), x => options.Request.GetQueryParameter(x));
			if (filters.Any())
			{
				return (IQueryable<T>)FilterByAttributeFormPropertyInfo.MakeGenericMethod(typeof(T)).Invoke(this, new object[] { query, filters });
			}
			return query;
		}
	}
}
