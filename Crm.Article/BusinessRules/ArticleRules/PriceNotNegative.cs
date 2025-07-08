namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class PriceNotNegative : NotNegativeRule<Article>
	{
		public PriceNotNegative()
		{
			Init(a => a.Price);
		}
	}
}