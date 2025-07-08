namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class ItemNoMaxLength : MaxLengthRule<Article>
	{
		public ItemNoMaxLength()
		{
			Init(a => a.ItemNo, 50);
		}
	}
}