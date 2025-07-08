namespace Crm.BusinessRules.RelationshipRules
{
	using Crm.Library.BaseModel;
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;

	[Rule]
	public class ParentIdRequired : RequiredRule<RelationshipBase>
	{
		public ParentIdRequired()
		{
			Init(r => r.ParentId);
		}
	}
}
