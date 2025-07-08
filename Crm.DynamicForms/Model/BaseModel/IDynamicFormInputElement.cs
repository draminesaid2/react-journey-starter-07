namespace Crm.DynamicForms.Model.BaseModel
{
	using Crm.Library.Validation.BaseRules;

	public interface IDynamicFormInputElement<T> : IHasRequiredProperty
	{
		T Response { get; set; }
	}
}
