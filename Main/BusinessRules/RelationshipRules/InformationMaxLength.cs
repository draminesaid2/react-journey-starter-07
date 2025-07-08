namespace Crm.BusinessRules.RelationshipRules
{
	using Crm.Library.BaseModel;
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;

	[Rule]
	public class InformationMaxLength : MaxLengthRule<RelationshipBase>
	{
		public InformationMaxLength()
		{
			Init(r => r.Information, 500, "RelationshipInformation");
		}
	}
}
