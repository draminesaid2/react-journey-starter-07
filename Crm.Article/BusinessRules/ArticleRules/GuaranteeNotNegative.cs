namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class GuaranteeNegative : NotNegativeRule<Article>
	{
		public GuaranteeNegative()
		{
			Init(a => a.GuaranteeInMonths);
		}
	}
}