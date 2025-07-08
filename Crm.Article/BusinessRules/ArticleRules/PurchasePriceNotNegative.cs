namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class PurchasePriceNotNegative : NotNegativeRule<Article>
	{
		public PurchasePriceNotNegative()
		{
			Init(a => a.PurchasePrice);
		}
	}
}