namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Model;

	[RestTypeFor(DomainType = typeof(FileResource))]
	public class FileResourceRest : RestEntityWithExtensionValues
	{
		public Guid? ParentId { get; set; }
		public string Filename { get; set; }
		public string Path { get; set; }
		public string ContentType { get; set; }
		[RestrictedField]
		public string Content { get; set; }
		public long Length { get; set; }
		public bool OfflineRelevant { get; set; }
		[ExplicitExpand]
		[NotReceived]
		public DocumentAttributeRest[] DocumentAttributes { get; set; }
	}
}
