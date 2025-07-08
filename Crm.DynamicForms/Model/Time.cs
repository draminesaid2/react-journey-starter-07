namespace Crm.DynamicForms.Model
{
	using System;

	using Crm.DynamicForms.Model.BaseModel;

	public class Time : DynamicFormElement, IDynamicFormInputElement<TimeSpan?>
	{
		public static string DiscriminatorValue = "Time";

		public virtual bool Required { get; set; }
		public virtual TimeSpan? Response { get; set; }

		public Time()
		{
			Size = 4;
	}
	}
}
