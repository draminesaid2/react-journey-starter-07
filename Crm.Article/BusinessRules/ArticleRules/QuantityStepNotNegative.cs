namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class QuantityStepNotNegative : NotNegativeRule<Article>
	{
		public QuantityStepNotNegative()
		{
			Init(a => a.QuantityStep);
		}
	}
}