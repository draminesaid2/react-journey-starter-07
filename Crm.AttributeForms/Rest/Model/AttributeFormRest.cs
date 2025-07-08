namespace Crm.AttributeForms.Rest.Model
{
	using Crm.AttributeForms.Model;
	using Crm.DynamicForms.Rest.Model;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(AttributeForm))]
	public class AttributeFormRest : DynamicFormReferenceRest
	{
	}
}
