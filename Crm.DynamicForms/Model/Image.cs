namespace Crm.DynamicForms.Model
{
	using System;

	using Crm.DynamicForms.Model.BaseModel;
	using Crm.Model;

	public class Image : DynamicFormElement, IExplicitElementOutputType
	{
		public static string DiscriminatorValue = "Image";
		public virtual Type ElementOutputType => typeof(FileResource);
		public virtual Guid? FileResourceId { get; set; }

		public Image()
		{
			Size = 2;
		}
	}
}