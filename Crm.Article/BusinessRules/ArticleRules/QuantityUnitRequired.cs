namespace Crm.Article.BusinessRules.ArticleRules
{
	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Library.Validation.BaseRules;

	public class QuantityUnitRequired : RequiredRule<Article>
	{
		public QuantityUnitRequired()
		{
			Init(a => a.QuantityUnitKey, "QuantityUnit");
		}

		protected override bool IsIgnoredFor(Article entity)
		{
			return entity.ArticleTypeKey != ArticleType.MaterialKey && entity.ArticleTypeKey != ArticleType.CostKey;
		}
	}
}
