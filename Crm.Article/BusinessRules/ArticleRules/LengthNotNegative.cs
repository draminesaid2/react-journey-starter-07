namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class LengthNotNegative : NotNegativeRule<Article>
	{
		public LengthNotNegative()
		{
			Init(a => a.Length);
		}
	}
}