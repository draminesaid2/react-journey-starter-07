namespace Crm.DynamicForms.Model
{
	using Crm.DynamicForms.Model.BaseModel;

	public class MultiLineText : DynamicFormElement, IDynamicFormInputElement<string>
	{
		public static string DiscriminatorValue = "MultiLineText";

		public virtual int MinLength { get; set; }
		public virtual int MaxLength { get; set; }

		public virtual bool Required { get; set; }
		public virtual string Response { get; set; }
		public virtual int RowSize { get; set; }

		public MultiLineText()
		{
			RowSize = 1;
			Size = 2;
		}
	}
}
