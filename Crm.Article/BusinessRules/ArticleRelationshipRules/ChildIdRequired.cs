namespace Crm.Article.BusinessRules.ArticleRelationshipRules
{
	using Crm.Article.Model.Relationships;
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;

	[Rule]
	public class ChildIdRequired : RequiredRule<ArticleRelationship>
	{
		public ChildIdRequired()
		{
			Init(r => r.ChildId, "Article");
		}
	}
}