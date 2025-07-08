namespace Crm.DynamicForms.Model
{
	using System;
	using System.Globalization;

	using Crm.DynamicForms.Model.BaseModel;
	using Crm.DynamicForms.ViewModels;

	using Newtonsoft.Json;

	public class FileAttachmentDynamicFormElement : DynamicFormElement, IDynamicFormInputElement<FileAttachmentResponse>, IExplicitResponseOutputType
	{
		public static string DiscriminatorValue = "FileAttachmentDynamicFormElement";
		public virtual bool Required { get; set; }
		public virtual int MinUploadCount { get; set; }
		public virtual int MaxUploadCount { get; set; }
		public virtual int MaxImageWidth { get; set; }
		public virtual int MaxImageHeight { get; set; }
		public virtual int MaxFileSize { get; set; }
		public virtual string MaxFileSizeInMb => string.Format(CultureInfo.InvariantCulture, "{0:0.#}MB", (double)MaxFileSize / 1024 / 1024);
		public virtual FileAttachmentResponse Response { get; set; } = new FileAttachmentResponse();
		public virtual Type ResponseOutputType => typeof(FileAttachmentResponseOutput);
		public override string ParseFromClient(string value)
		{
			if (value == null)
			{
				return null;
			}
			var response = JsonConvert.DeserializeObject<FileAttachmentResponse>(value);
			return JsonConvert.SerializeObject(response);
		}
		public override string ParseToClient(string value)
		{
			if (value == null)
			{
				return null;
			}
			var response = JsonConvert.DeserializeObject<FileAttachmentResponse>(value);
			return JsonConvert.SerializeObject(response);
		}

		public FileAttachmentDynamicFormElement()
		{
			Size = 2;
	}
	}
}
