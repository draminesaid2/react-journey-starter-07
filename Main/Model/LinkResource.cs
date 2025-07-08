namespace Crm.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class LinkResource : EntityBase<Guid>, ISoftDelete
	{
		// Properties
		public virtual Guid ParentId { get; set; }
		public virtual string Url { get; set; }
		public virtual string Description { get; set; }
	}
}