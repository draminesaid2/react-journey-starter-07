namespace Crm.DynamicForms.BusinessRules.DynamicForm
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Validation.BaseRules;

	public class CategoryKeyRequired : RequiredRule<DynamicForm>
	{
		public CategoryKeyRequired()
		{
			Init(x => x.CategoryKey);
		}
	}
}