namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class WidthNegative : NotNegativeRule<Article>
	{
		public WidthNegative()
		{
			Init(a => a.Width);
		}
	}
}