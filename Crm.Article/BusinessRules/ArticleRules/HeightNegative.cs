namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class HeightNegative : NotNegativeRule<Article>
	{
		public HeightNegative()
		{
			Init(a => a.Height);
		}
	}
}