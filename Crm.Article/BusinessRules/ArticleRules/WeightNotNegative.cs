namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class WeightNotNegative : NotNegativeRule<Article>
	{
		public WeightNotNegative()
		{
			Init(a => a.Weight);
		}
	}
}