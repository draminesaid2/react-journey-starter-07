namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Model;
	using Crm.Model.Enums;

	[RestTypeFor(DomainType = typeof(DocumentAttribute))]
	public class DocumentAttributeRest : RestEntityWithExtensionValues
	{
		public virtual ReferenceType ReferenceType { get; set; }
		public virtual string DocumentCategoryKey { get; set; }
		public virtual Guid ReferenceKey { get; set; }
		public virtual string Description { get; set; }
		public virtual string LongText { get; set; }
		public virtual bool OfflineRelevant { get; set; }
		public virtual bool UseForThumbnail { get; set; }
		public virtual Guid? FileResourceKey { get; set; }
		public virtual string FileName { get; set; }
		public virtual long Length { get; set; }
		[ExplicitExpand]
		[NotReceived]
		public FileResourceRest FileResource { get; set; }
	}
}
