namespace Crm.DynamicForms.Model
{
	using Crm.DynamicForms.Model.BaseModel;

	public class SignaturePad : DynamicFormElement, IDynamicFormInputElement<string>
	{
		public static string DiscriminatorValue = "SignaturePad";

		public virtual bool Required { get; set; }
		public virtual string Response { get; set; }

		public SignaturePad()
		{
			Size = 2;
	}
	}
}
