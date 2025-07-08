namespace Crm.Article.Services;

using System;
using System.Linq;

using Crm.Article.Model.Relationships;
using Crm.Library.BaseModel.Interfaces;
using Crm.Library.Data.Domain.DataInterfaces;
using Crm.Model;

public class CrmMerger : IMerger<Company>
{
	private readonly IRepositoryWithTypedId<ArticleCompanyRelationship, Guid> articleCompanyRelationshipRepository;
	public CrmMerger(IRepositoryWithTypedId<ArticleCompanyRelationship, Guid> articleCompanyRelationshipRepository)
	{
		this.articleCompanyRelationshipRepository = articleCompanyRelationshipRepository;
	}
	public virtual void Merge(Company loser, Company winner)
	{
		MergeArticleCompanyRelationships(loser, winner);
	}
	protected virtual void MergeArticleCompanyRelationships(Company loser, Company winner)
	{
		var looserArticleCompanyRelationships = articleCompanyRelationshipRepository.GetAll().Where(x => x.ChildId == loser.Id);
		foreach (var looserArticleCompanyRelationship in looserArticleCompanyRelationships)
		{
			looserArticleCompanyRelationship.ChildId = winner.Id;
			articleCompanyRelationshipRepository.SaveOrUpdate(looserArticleCompanyRelationship);
		}
	}
}
