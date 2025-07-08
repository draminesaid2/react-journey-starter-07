namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Library.Validation.BaseRules;

	public class TypeRequired : RequiredRule<Article>
	{
		public TypeRequired()
		{
			Init(a => a.ArticleTypeKey, propertyNameReplacementKey: "ArticleType");
		}
	}
}
