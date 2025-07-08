namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class DescriptionRequired : RequiredRule<Article>
	{
		public DescriptionRequired()
		{
			Init(a => a.Description);
		}
	}
}