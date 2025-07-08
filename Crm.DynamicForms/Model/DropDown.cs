namespace Crm.DynamicForms.Model
{
	using Crm.DynamicForms.Model.BaseModel;

	public class DropDown : DynamicFormElement, IDynamicFormInputElement<int?>, IDynamicFormMultipleChoiceElement
	{
		public static string DiscriminatorValue = "DropDown";

		public virtual bool Required { get; set; }
		public virtual int? Response { get; set; }
		public virtual int Choices { get; set; }
		public virtual bool Randomized { get; set; }
		public virtual int RowSize { get; set; }

		public DropDown()
		{
			Choices = 3;
			RowSize = 1;
			Size = 2;
		}
	}
}
