namespace Crm.Article
{
	using System;
	using System.Collections.Generic;

    using Crm.Article.Model.Lookups;
    using Crm.Article.Services.Interfaces;
    using Crm.Library.Globalization.Lookup;

	public class ArticleUsedLookupsProvider : IUsedLookupsProvider
    {
        private readonly IArticleService articleService;
        public ArticleUsedLookupsProvider(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public virtual IEnumerable<object> GetUsedLookupKeys(Type lookupType)
		{
			if (lookupType == typeof(ArticleType))
			{
				return articleService.GetUsedArticleTypes();
			}

			if (lookupType == typeof(ArticleGroup01))
			{
				return articleService.GetUsedArticleGroup01s();
			}

			if (lookupType == typeof(ArticleGroup02))
			{
				return articleService.GetUsedArticleGroup02s();
			}

			if (lookupType == typeof(ArticleGroup03))
			{
				return articleService.GetUsedArticleGroup03s();
			}

			if (lookupType == typeof(ArticleGroup04))
			{
				return articleService.GetUsedArticleGroup04s();
			}

			if (lookupType == typeof(ArticleGroup05))
			{
				return articleService.GetUsedArticleGroup05s();
			}

			if (lookupType == typeof(ArticleRelationshipType))
			{
				return articleService.GetUsedArticleRelationshipTypes();
			}

			if (lookupType == typeof(QuantityUnit))
			{
				return articleService.GetUsedQuantityUnits();
			}

			if (lookupType == typeof(VATLevel))
			{
				return articleService.GetUsedVATLevels();
			}

			return new List<object>();
		}
	}
}
