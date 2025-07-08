namespace Crm.BusinessRules.RelationshipRules
{
	using System.Linq;

	using Crm.Library.BaseModel;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Validation;

	[Rule]
	public class LookupRelationshipMustBeUnique : Rule<LookupRelationshipBase>
	{
		private readonly IRepository<LookupRelationshipBase> repository;
		public LookupRelationshipMustBeUnique(IRepository<LookupRelationshipBase> repository)
			: base(RuleClass.Unique)
		{
			this.repository = repository;
		}
		protected override RuleViolation CreateRuleViolation(LookupRelationshipBase entity)
		{
			return RuleViolation(entity, x => x.ChildId, x => x.ParentId, propertyNameReplacementKey: "Relationship");
		}
		public override bool IsSatisfiedBy(LookupRelationshipBase entity)
		{
			var conflictingRelationships = repository.GetAll()
				.Where(x => x.ParentId == entity.ParentId && x.ChildId == entity.ChildId && x.Id != entity.Id
					&& x.RelationshipTypeKey == entity.RelationshipTypeKey);
			if (conflictingRelationships.Any())
			{
				return false;
			}
			return true;
		}
	}
}
