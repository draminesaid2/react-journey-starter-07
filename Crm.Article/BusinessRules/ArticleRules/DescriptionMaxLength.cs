namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class DescriptionMaxLength : MaxLengthRule<Article>
	{
		public DescriptionMaxLength()
		{
			Init(a => a.Description, 450);
		}
	}
}