namespace Crm.Article.BusinessRules.ProductFamilyRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class StatusRequired : RequiredRule<ProductFamily>
	{
		public StatusRequired()
		{
			Init(c=> c.StatusKey);
		}
	}
}