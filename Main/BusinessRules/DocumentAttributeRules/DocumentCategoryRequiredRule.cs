namespace Crm.BusinessRules.DocumentAttributeRules
{
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class DocumentCategoryRequiredRule : RequiredRule<DocumentAttribute>
	{
		public DocumentCategoryRequiredRule()
		{
			Init(x => x.DocumentCategoryKey, "Category");
		}
	}
}