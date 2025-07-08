namespace Crm.BusinessRules.RelationshipRules
{
	using Crm.Library.BaseModel;
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;

	[Rule]
	public class RelationshipTypeKeyRequired : RequiredRule<LookupRelationshipBase>
	{
		public RelationshipTypeKeyRequired()
		{
			Init(r => r.RelationshipTypeKey);
		}
	}
}
