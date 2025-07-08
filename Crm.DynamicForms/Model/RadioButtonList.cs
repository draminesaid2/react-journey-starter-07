namespace Crm.DynamicForms.Model
{
	using Crm.DynamicForms.Model.BaseModel;

	public class RadioButtonList : DynamicFormElement, IDynamicFormInputElement<int?>, IDynamicFormMultipleChoiceElement
	{
		public static string DiscriminatorValue = "RadioButtonList";

		public virtual bool Required { get; set; }
		public virtual int? Response { get; set; }
		public virtual int Choices { get; set; }
		public virtual bool Randomized { get; set; }
		public virtual int Layout { get; set; }

		public RadioButtonList()
		{
			Choices = 3;
			Size = 2;
		}
	}
}
