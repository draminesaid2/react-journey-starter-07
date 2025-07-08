namespace Crm.Article.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Article.Model.Relationships;
	using Crm.Article.Services.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;

	using NHibernate.Criterion;
	using NHibernate.Transform;

	public class ArticleService : IArticleService
	{
		private readonly IRepositoryWithTypedId<Article, Guid> articleRepository;
		private readonly IRepositoryWithTypedId<ArticleRelationship, Guid> articleRelationshipRepository;
		private readonly ILookupManager lookupManager;
		private readonly IResourceManager resourceManager;

		// Methods
		public virtual IQueryable<Article> GetArticles()
		{
			return articleRepository.GetAll();
		}

		public virtual Article GetArticle(Guid id)
		{
			return articleRepository.Get(id);
		}

		public virtual Article GetArticleByItemNo(string itemNo)
		{
			return articleRepository.GetAll().FirstOrDefault(a => a.ItemNo == itemNo);
		}

		public virtual IList<ArticleGroupWithChildren> CategoryHierarchy()
		{
			var res = new List<ArticleGroupWithChildren>();

			var criteria = articleRepository.Session.CreateCriteria(typeof(Article));
			criteria.Add(Restrictions.Eq(nameof(Article.ArticleTypeKey), ArticleType.MaterialKey));
			criteria.Add(Restrictions.Eq(nameof(Article.IsActive), true));
			criteria.AddOrder(Order.Desc(Projections.Property(nameof(Article.ArticleGroup01Key))));
			criteria.AddOrder(Order.Desc(Projections.Property(nameof(Article.ArticleGroup02Key))));
			criteria.AddOrder(Order.Desc(Projections.Property(nameof(Article.ArticleGroup03Key))));
			criteria.AddOrder(Order.Desc(Projections.Property(nameof(Article.ArticleGroup04Key))));
			criteria.AddOrder(Order.Desc(Projections.Property(nameof(Article.ArticleGroup05Key))));
			criteria.SetProjection(
				Projections.Distinct(Projections.ProjectionList()
					.Add(Projections.Alias(Projections.Property(nameof(Article.ArticleGroup01Key)), nameof(Article.ArticleGroup01Key)))
					.Add(Projections.Alias(Projections.Property(nameof(Article.ArticleGroup02Key)), nameof(Article.ArticleGroup02Key)))
					.Add(Projections.Alias(Projections.Property(nameof(Article.ArticleGroup03Key)), nameof(Article.ArticleGroup03Key)))
					.Add(Projections.Alias(Projections.Property(nameof(Article.ArticleGroup04Key)), nameof(Article.ArticleGroup04Key)))
					.Add(Projections.Alias(Projections.Property(nameof(Article.ArticleGroup05Key)), nameof(Article.ArticleGroup05Key)))));

			criteria.SetResultTransformer(
				new AliasToBeanResultTransformer(typeof(ArticleGroupInformation)));

			var distinctArticleGroupCombinations = criteria.List().Cast<ArticleGroupInformation>().ToList();
			var unspecified = resourceManager.GetTranslation("Unspecified");
			foreach (ArticleGroupInformation combination in distinctArticleGroupCombinations)
			{
				if (string.IsNullOrEmpty(combination.ArticleGroup01Key) && string.IsNullOrEmpty(combination.ArticleGroup02Key) && string.IsNullOrEmpty(combination.ArticleGroup03Key) && string.IsNullOrEmpty(combination.ArticleGroup04Key) && string.IsNullOrEmpty(combination.ArticleGroup05Key))
				{
					continue;
				}
				ArticleGroupWithChildren firstLevel;
				var firstLevelExisting = res.Where(r => r.Key == combination.ArticleGroup01Key).ToList();
				if (firstLevelExisting.Any())
				{
					firstLevel = firstLevelExisting.First();
				}
				else
				{
					ArticleGroup01 articleGroup01 = new ArticleGroup01();
					if (string.IsNullOrEmpty(combination.ArticleGroup01Key))
						articleGroup01.Value = unspecified;
					else
						articleGroup01 = lookupManager.Get<ArticleGroup01>(combination.ArticleGroup01Key);
					var articleImage = articleGroup01?.Base64Image;
					if (articleImage is null)
					{
						articleImage = articleRepository.GetAll().Where(x => x.ArticleGroup01Key == combination.ArticleGroup01Key && x.DocumentAttributes.Count > 0).SelectMany(x => x.DocumentAttributes).Select(x => Convert.ToBase64String(x.FileResource.Content)).FirstOrDefault();
					}
					firstLevel = new ArticleGroupWithChildren(combination.ArticleGroup01Key, articleGroup01?.Value, articleImage, string.IsNullOrEmpty(combination.ArticleGroup01Key));
					res.Add(firstLevel);
					res = res.OrderBy(children => children.Value).ToList();
				}

				if (string.IsNullOrEmpty(combination.ArticleGroup02Key) && string.IsNullOrEmpty(combination.ArticleGroup03Key) && string.IsNullOrEmpty(combination.ArticleGroup04Key) && string.IsNullOrEmpty(combination.ArticleGroup05Key))
				{
					continue;
				}
				var secondLevel = firstLevel.Subgroups.FirstOrDefault(r => r.Key == combination.ArticleGroup02Key);
				if (secondLevel == null)
				{
					ArticleGroup02 articleGroup02 = new ArticleGroup02();
					if (string.IsNullOrEmpty(combination.ArticleGroup02Key))
						articleGroup02.Value = unspecified;
					else
						articleGroup02 = lookupManager.Get<ArticleGroup02>(combination.ArticleGroup02Key);
					var articleImage = articleGroup02?.Base64Image;
					if (articleImage is null)
					{
						articleImage = articleRepository.GetAll().Where(x => x.ArticleGroup01Key == combination.ArticleGroup01Key && x.ArticleGroup02Key == combination.ArticleGroup02Key && x.DocumentAttributes.Count > 0).SelectMany(x => x.DocumentAttributes).Select(x => Convert.ToBase64String(x.FileResource.Content)).FirstOrDefault();
					}
					secondLevel = new ArticleGroupWithChildren(combination.ArticleGroup02Key, articleGroup02?.Value, articleImage, string.IsNullOrEmpty(combination.ArticleGroup02Key));
					firstLevel.Subgroups.Add(secondLevel);
					firstLevel.Subgroups = firstLevel.Subgroups.OrderBy(children => children.Value).ToList();
				}

				if (string.IsNullOrEmpty(combination.ArticleGroup03Key) && string.IsNullOrEmpty(combination.ArticleGroup04Key) && string.IsNullOrEmpty(combination.ArticleGroup05Key))
				{
					continue;
				}
				var thirdLevel = secondLevel.Subgroups.FirstOrDefault(r => r.Key == combination.ArticleGroup03Key);
				if (thirdLevel == null)
				{
					ArticleGroup03 articleGroup03 = new ArticleGroup03();
					if (string.IsNullOrEmpty(combination.ArticleGroup03Key))
						articleGroup03.Value = unspecified;
					else
						articleGroup03 = lookupManager.Get<ArticleGroup03>(combination.ArticleGroup03Key);
					var articleImage = articleGroup03?.Base64Image;
					if (articleImage is null)
					{
						articleImage = articleRepository.GetAll().Where(x => x.ArticleGroup01Key == combination.ArticleGroup01Key && x.ArticleGroup02Key == combination.ArticleGroup02Key && x.ArticleGroup03Key == combination.ArticleGroup03Key && x.DocumentAttributes.Count > 0).SelectMany(x => x.DocumentAttributes).Select(x => Convert.ToBase64String(x.FileResource.Content)).FirstOrDefault();
					}
					thirdLevel = new ArticleGroupWithChildren(combination.ArticleGroup03Key, articleGroup03?.Value, articleImage, string.IsNullOrEmpty(combination.ArticleGroup03Key));
					secondLevel.Subgroups.Add(thirdLevel);
					secondLevel.Subgroups = secondLevel.Subgroups.OrderBy(children => children.Value).ToList();
				}

				if (string.IsNullOrEmpty(combination.ArticleGroup04Key) && string.IsNullOrEmpty(combination.ArticleGroup05Key))
				{
					continue;
				}
				var fourthLevel = thirdLevel.Subgroups.FirstOrDefault(r => r.Key == combination.ArticleGroup04Key);
				if (fourthLevel == null)
				{
					ArticleGroup04 articleGroup04 = new ArticleGroup04();
					if (string.IsNullOrEmpty(combination.ArticleGroup04Key))
						articleGroup04.Value = unspecified;
					else
						articleGroup04 = lookupManager.Get<ArticleGroup04>(combination.ArticleGroup04Key);
					var articleImage = articleGroup04?.Base64Image;
					if (articleImage is null)
					{
						articleImage = articleRepository.GetAll().Where(x => x.ArticleGroup01Key == combination.ArticleGroup01Key && x.ArticleGroup02Key == combination.ArticleGroup02Key && x.ArticleGroup03Key == combination.ArticleGroup03Key && x.ArticleGroup04Key == combination.ArticleGroup04Key && x.DocumentAttributes.Count > 0).SelectMany(x => x.DocumentAttributes).Select(x => Convert.ToBase64String(x.FileResource.Content)).FirstOrDefault();
					}
					fourthLevel = new ArticleGroupWithChildren(combination.ArticleGroup04Key, articleGroup04?.Value, articleImage, string.IsNullOrEmpty(combination.ArticleGroup04Key));
					thirdLevel.Subgroups.Add(fourthLevel);
					thirdLevel.Subgroups = thirdLevel.Subgroups.OrderBy(children => children.Value).ToList();
				}

				if (string.IsNullOrEmpty(combination.ArticleGroup05Key))
				{
					continue;
				}
				var fifthLevel = fourthLevel.Subgroups.FirstOrDefault(r => r.Key == combination.ArticleGroup05Key);
				if (fifthLevel == null)
				{
					ArticleGroup05 articleGroup05 = new ArticleGroup05();
					if (string.IsNullOrEmpty(combination.ArticleGroup05Key))
						articleGroup05.Value = unspecified;
					else
						articleGroup05 = lookupManager.Get<ArticleGroup05>(combination.ArticleGroup05Key);
					var articleImage = articleGroup05?.Base64Image;
					if (articleImage is null)
					{
						articleImage = articleRepository.GetAll().Where(x => x.ArticleGroup01Key == combination.ArticleGroup01Key && x.ArticleGroup02Key == combination.ArticleGroup02Key && x.ArticleGroup03Key == combination.ArticleGroup03Key && x.ArticleGroup04Key == combination.ArticleGroup04Key && x.ArticleGroup05Key == combination.ArticleGroup05Key  && x.DocumentAttributes.Count > 0).SelectMany(x => x.DocumentAttributes).Select(x => Convert.ToBase64String(x.FileResource.Content)).FirstOrDefault();
					}
					fourthLevel.Subgroups.Add(new ArticleGroupWithChildren(combination.ArticleGroup05Key, articleGroup05?.Value, articleImage, string.IsNullOrEmpty(combination.ArticleGroup05Key)));
					fourthLevel.Subgroups = fourthLevel.Subgroups.OrderBy(children => children.Value).ToList();
				}
			}

			return res;
		}

		public virtual IEnumerable<string> GetUsedArticleTypes()
		{
			return articleRepository.GetAll().Select(a => a.ArticleTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedArticleGroup01s()
		{
			return articleRepository.GetAll().Select(a => a.ArticleGroup01Key).Distinct();
		}

		public virtual IEnumerable<string> GetUsedArticleGroup02s()
		{
			return articleRepository.GetAll().Select(a => a.ArticleGroup02Key).Distinct();
		}

		public virtual IEnumerable<string> GetUsedArticleGroup03s()
		{
			return articleRepository.GetAll().Select(a => a.ArticleGroup03Key).Distinct();
		}

		public virtual IEnumerable<string> GetUsedArticleGroup04s()
		{
			return articleRepository.GetAll().Select(a => a.ArticleGroup04Key).Distinct();
		}

		public virtual IEnumerable<string> GetUsedArticleGroup05s()
		{
			return articleRepository.GetAll().Select(a => a.ArticleGroup05Key).Distinct();
		}

		public virtual IEnumerable<string> GetUsedArticleRelationshipTypes()
		{
			return articleRelationshipRepository.GetAll().Select(a => a.RelationshipTypeKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedQuantityUnits()
		{
			return articleRepository.GetAll().Select(a => a.QuantityUnitKey).Distinct().ToList()
				.Union(articleRelationshipRepository.GetAll().Select(a => a.QuantityUnitKey).Distinct().ToList());
		}

		public virtual IEnumerable<string> GetUsedVATLevels()
		{
			return articleRepository.GetAll().Select(a => a.VATLevelKey).Distinct();
		}

		public ArticleService(IRepositoryWithTypedId<Article, Guid> articleRepository, IRepositoryWithTypedId<ArticleRelationship, Guid> articleRelationshipRepository, ILookupManager lookupManager, IResourceManager resourceManager)
		{
			this.articleRepository = articleRepository;
			this.articleRelationshipRepository = articleRelationshipRepository;
			this.lookupManager = lookupManager;
			this.resourceManager = resourceManager;
		}
	}
}
