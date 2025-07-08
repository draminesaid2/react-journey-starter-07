namespace Crm.AttributeForms.Model
{
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Attributes;

	using Crm.DynamicForms.Model;

	public class DynamicFormExtension : EntityExtension<DynamicForm>
	{
		[UI(Hidden = true)]
		public virtual string ContactType { get; set; }
	}
}