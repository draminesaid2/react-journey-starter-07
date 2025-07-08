namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class WeightNetNotNegative : NotNegativeRule<Article>
	{
		public WeightNetNotNegative()
		{
			Init(a => a.WeightNet);
		}
	}
}