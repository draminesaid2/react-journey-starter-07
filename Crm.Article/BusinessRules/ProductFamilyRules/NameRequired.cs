namespace Crm.Article.BusinessRules.ProductFamilyRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;
	public class NameRequired : RequiredRule<ProductFamily>
	{
		public NameRequired()
		{
			Init(c => c.Name);
		}
	}
}