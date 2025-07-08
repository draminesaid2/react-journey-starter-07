namespace Crm.Controllers.OData
{
	using System;
	using System.Linq;

	using Crm.Library.Api;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Api.Controller;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Model;
	using Crm.Rest.Model;
	using Microsoft.AspNetCore.OData.Query;
	using Microsoft.AspNetCore.Mvc;

	[ControllerName("Main_Tag")]
	public class TagODataController : ODataControllerEx, IEntityApiController
	{
		private readonly IRepositoryWithTypedId<Tag, Guid> tagRepository;
		public Type EntityType => typeof(Tag);
		public TagODataController(IRepositoryWithTypedId<Tag, Guid> tagRepository)
		{
			this.tagRepository = tagRepository;
		}
		[HttpGet]
		[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Top | AllowedQueryOptions.Skip)]
		public virtual IActionResult GetUniqueTagNames(ODataQueryOptions<TagRest> options) => GetUniqueTagNames(options, null, (string)null);

		[HttpGet]
		[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Top | AllowedQueryOptions.Skip)]
		public virtual IActionResult GetUniqueTagNames(ODataQueryOptions<TagRest> options, string ContactType) => GetUniqueTagNames(options, ContactType, (string)null);

		[HttpGet]
		[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Top | AllowedQueryOptions.Skip)]
		public virtual IActionResult GetUniqueTagNames(ODataQueryOptions<TagRest> options, string Filter, int? _) => GetUniqueTagNames(options, null, Filter);

		[HttpGet]
		[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Top | AllowedQueryOptions.Skip)]
		public virtual IActionResult GetUniqueTagNames(ODataQueryOptions<TagRest> options, string ContactType, string Filter)
		{
			ContactType = ContactType?.Trim();
			Filter = Filter?.Trim();
			var tags = tagRepository.GetAll();
			if (string.IsNullOrEmpty(ContactType) == false)
			{
				tags = tags.Where(x => x.Contact.ContactType == ContactType);
			}
			if (string.IsNullOrEmpty(Filter) == false)
			{
				tags = tags.Where(x => x.Name.Contains(Filter));
			}
			var names = tags
				.OrderBy(x => x.Name)
				.Select(x => x.Name)
				.Distinct();
			if (options.Top != null)
			{
				names = options.Top.ApplyTo(names, new ODataQuerySettings());
			}
			if (options.Skip != null)
			{
				names = options.Skip.ApplyTo(names, new ODataQuerySettings());
			}
			return Ok(names);
		}
	}
}
