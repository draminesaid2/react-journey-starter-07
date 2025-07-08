namespace Crm.DynamicForms.Model.BaseModel
{
	public interface IDynamicFormElement
	{
		string ParseFromClient(string value);
		string ParseToClient(string value);
	}
}