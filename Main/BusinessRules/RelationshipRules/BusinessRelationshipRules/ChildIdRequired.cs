namespace Crm.BusinessRules.RelationshipRules.BusinessRelationshipRules
{
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;
	using Crm.Model.Relationships;

	[Rule]
	public class ChildIdRequired : RequiredRule<BusinessRelationship>
	{
		public ChildIdRequired()
		{
			Init(r => r.ChildId, "Company");
		}
	}
}