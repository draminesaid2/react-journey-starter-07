namespace Crm.BusinessRules.CompanyPersonRelationship
{

	using Crm.Library.Validation.BaseRules;
	using Crm.Model.Relationships;
	public class ChildIdRequired : RequiredRule<CompanyPersonRelationship>
	{
		public ChildIdRequired()
		{
			Init(x => x.ChildId, "Person");
		}

	}
}
