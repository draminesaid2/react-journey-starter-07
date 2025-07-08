namespace Crm.DynamicForms.Model.BaseModel
{
	public interface IDynamicFormMultipleChoiceElement
	{
		int Choices { get; set; }
		bool Randomized { get; set; }
	}
}
