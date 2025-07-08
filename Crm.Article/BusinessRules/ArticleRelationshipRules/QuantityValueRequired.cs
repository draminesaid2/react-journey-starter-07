namespace Crm.Article.BusinessRules.ArticleRelationshipRules
{
	using Crm.Article.Model.Relationships;
	using Crm.Library.Validation.BaseRules;

	public class QuantityValueRequired : RequiredRule<ArticleRelationship>
	{
		public QuantityValueRequired()
		{
			Init(a => a.QuantityValue);
		}
		protected override bool IsIgnoredFor(ArticleRelationship entity)
		{
			return entity.RelationshipType == null || !entity.RelationshipType.HasQuantity;
		}
	}
}