namespace Crm.DynamicForms.BusinessRules.DynamicFormReference
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Validation.BaseRules;

	public class ReferenceKeyRequired : RequiredRule<DynamicFormReference>
	{
		public ReferenceKeyRequired()
		{
			Init(x => x.ReferenceKey);
		}
	}
}