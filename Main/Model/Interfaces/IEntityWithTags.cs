namespace Crm.Model.Interfaces
{
	using System.Collections.Generic;

	public interface IEntityWithTags
	{
		ICollection<Tag> Tags { get; set; }
	}
}