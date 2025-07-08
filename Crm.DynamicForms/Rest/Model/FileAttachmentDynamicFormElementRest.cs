namespace Crm.DynamicForms.Rest.Model
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(FileAttachmentDynamicFormElement))]
	public class FileAttachmentDynamicFormElementRest : DynamicFormElementRest, IDynamicFormInputElementRest
	{
		public bool Required { get; set; }
		public int MinUploadCount { get; set; }
		public int MaxUploadCount { get; set; }
		public int MaxImageWidth { get; set; }
		public int MaxImageHeight { get; set; }
		public int MaxFileSize { get; set; }
		[RestrictedField, NotReceived]
		public string DefaultResponseValue
		{
			get => "[]";
			set { }
		}
	}
}
