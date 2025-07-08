namespace Crm.Article.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Article.Model;
	using Crm.Library.AutoFac;

	public interface IArticleService : ITransientDependency
	{
		IQueryable<Article> GetArticles();
		Article GetArticle(Guid id);
		Article GetArticleByItemNo(string itemNo);
		/// <summary>
		/// When a customers articles have nested categories, we save the category
		/// paths for each article to the ArticleGroup[1-5] columns.
		/// This function figures the available paths from all saved articles and
		/// returns the nested categories in a JSON friendly data structure.
		/// </summary>
		/// <returns>
		/// The list of parent article groups (aka Categories) with sub-groups
		/// (Subcategories) up to the 5th level
		/// </returns>
		IList<ArticleGroupWithChildren> CategoryHierarchy();
		IEnumerable<string> GetUsedArticleTypes();
		IEnumerable<string> GetUsedArticleGroup01s();
		IEnumerable<string> GetUsedArticleGroup02s();
		IEnumerable<string> GetUsedArticleGroup03s();
		IEnumerable<string> GetUsedArticleGroup04s();
		IEnumerable<string> GetUsedArticleGroup05s();
		IEnumerable<string> GetUsedArticleRelationshipTypes();
		IEnumerable<string> GetUsedQuantityUnits();
		IEnumerable<string> GetUsedVATLevels();
	}
}
