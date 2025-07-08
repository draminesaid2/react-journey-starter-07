namespace Crm.AttributeForms.Model
{
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Attributes;

	using Crm.DynamicForms.Model.BaseModel;

	public class DynamicFormElementExtension : EntityExtension<DynamicFormElement>
	{
		[UI(Hidden = true)]
		public virtual bool DisplayOnItemTemplate { get; set; }

		[UI(Hidden = true)]
		public virtual bool Filterable { get; set; }
	}
}