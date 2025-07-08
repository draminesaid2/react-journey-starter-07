namespace Crm.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Crm.Controllers;
	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Model;
	using Crm.Model.Notes;

	using LinqKit;

	using Microsoft.AspNetCore.OData.Query;

	public class ODataQueryRelatedContactFilter : IODataQueryFunction, IDependency
	{
		private readonly IEnumerable<Lazy<IRelatedContact, DependencyMetadata>> relatedContacts;
		protected static MethodInfo RelatedContactFilterInfo = typeof(ODataQueryRelatedContactFilter)
			.GetMethod(nameof(RelatedContactFilter), BindingFlags.Instance | BindingFlags.NonPublic);
		private readonly IRepositoryWithTypedId<Contact, Guid> contactRepository;
		public ODataQueryRelatedContactFilter(IEnumerable<Lazy<IRelatedContact, DependencyMetadata>> relatedContacts, IRepositoryWithTypedId<Contact, Guid> contactRepository)
		{
			this.relatedContacts = relatedContacts;
			this.contactRepository = contactRepository;
		}

		protected virtual IQueryable<T> RelatedContactFilter<T>(IQueryable<T> query, Guid contactId)
			where T : class, IEntityWithContactId
		{
			var contact = contactRepository.Get(contactId);
			var requiredRelatedContactType = typeof(IRelatedContact<>).MakeGenericType(contact.ActualType);
			var relatedContactsForContactType = relatedContacts.Where(
					x => x.Metadata
						.RegisteredType
						.Implements(requiredRelatedContactType))
				.Select(x => x.Value);

			var predicate = PredicateBuilder.New<T>(false);
			predicate = predicate.Or(p => p.ContactId == contact.Id);

			foreach (var relatedContact in relatedContactsForContactType)
			{
				predicate = predicate.Or(p => relatedContact.RelatedContact(contact).Select(x => x.Id).Contains(p.ContactId.Value));
			}

			var combinedIQueryable = query.Where(predicate);

			return combinedIQueryable;
		}
		
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(Note).IsAssignableFrom(typeof(T)) == false && typeof(Task).IsAssignableFrom(typeof(T)) == false)
			{
				return query;
			}

			const string contactIdParameterName = "filterByRelatedContactId";
			var parameters = options.Request.Query;
			if (parameters.Keys.Contains(contactIdParameterName))
			{
				var contactId = Guid.Parse(options.Request.GetQueryParameter(contactIdParameterName));
				var genericMethod = RelatedContactFilterInfo.MakeGenericMethod(typeof(T));
				return (IQueryable<T>)genericMethod.Invoke(this, new object[] { query, contactId });
			}

			return query;
		}
	}
}
