namespace Crm.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Model.Enums;

	public class DocumentAttribute : EntityBase<Guid>, ISoftDelete
	{
		public virtual ReferenceType ReferenceType { get; set; }
		public virtual string DocumentCategoryKey { get; set; }
		public virtual Guid ReferenceKey { get; set; }
		public virtual Contact Contact { get; set; }
		public virtual string Description { get; set; }
		public virtual string LongText { get; set; }
		public virtual bool OfflineRelevant { get; set; }
		public virtual bool UseForThumbnail { get; set; }

		public virtual Guid? FileResourceKey { get; set; }
		public virtual string FileName { get; set; }
		public virtual long Length { get; set; }
		public virtual FileResource FileResource { get; set; }
	}
}