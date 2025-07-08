namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class ItemNoRequired : RequiredRule<Article>
	{
		public ItemNoRequired()
		{
			Init(a => a.ItemNo);
		}
	}
}