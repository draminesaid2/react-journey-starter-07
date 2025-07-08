using Microsoft.AspNetCore.Mvc;

namespace Crm.Rest.Controllers
{

	using Crm.Library.Extensions;
	using Crm.Library.Rest;
	using Crm.Services.Interfaces;

	public class TagRestController : RestController<string>
	{
		private readonly ITagService tagService;

		public TagRestController(ITagService tagService, RestTypeProvider restTypeProvider)
			: base(restTypeProvider)
		{
			this.tagService = tagService;
		}

		public virtual ActionResult List(string searchTag)
		{
			var tags = searchTag.IsNullOrEmpty()
				           ? tagService.GetAllTags(string.Empty)
				           : tagService.GetTags(searchTag);
			return Rest(tags, "Tags", "Tag");
		}
	}
}
