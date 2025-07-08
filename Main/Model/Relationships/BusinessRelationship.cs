namespace Crm.Model.Relationships
{
	using Crm.Library.BaseModel;
	using Crm.Model.Lookups;

	public class BusinessRelationship : LookupRelationshipWithInverse<Company, BusinessRelationshipType>
	{
	}
}