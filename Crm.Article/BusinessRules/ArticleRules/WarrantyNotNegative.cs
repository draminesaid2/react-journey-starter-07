namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class WarrantyNegative : NotNegativeRule<Article>
	{
		public WarrantyNegative()
		{
			Init(a => a.WarrantyInMonths);
		}
	}
}