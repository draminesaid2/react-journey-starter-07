namespace Crm.Article.BusinessRules.ArticleRules
{
	using System.Linq;

	using Crm.Article.Model;
	using Crm.Article.Services.Interfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Validation;

	public class ItemNoUnique : Rule<Article>
	{
		private readonly IArticleService articleService;

		// Methods
		protected override bool IsIgnoredFor(Article article)
		{
			return article.ItemNo.IsNullOrEmpty();
		}

		public override bool IsSatisfiedBy(Article article)
		{
			return !articleService
				.GetArticles()
				.Any(a => a.ItemNo == article.ItemNo && a.Id != article.Id);
		}

		protected override RuleViolation CreateRuleViolation(Article article)
		{
			return RuleViolation(article, a => a.ItemNo);
		}

		// Constructor
		public ItemNoUnique(IArticleService articleService)
			: base(RuleClass.Unique)
		{
			this.articleService = articleService;
		}
	}
}
