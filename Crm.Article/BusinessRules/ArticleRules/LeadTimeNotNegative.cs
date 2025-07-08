namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class LeadTimeNegative : NotNegativeRule<Article>
	{
		public LeadTimeNegative()
		{
			Init(a => a.LeadTimeInDays);
		}
	}
}