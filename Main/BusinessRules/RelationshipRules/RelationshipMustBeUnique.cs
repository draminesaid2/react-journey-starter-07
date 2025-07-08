namespace Crm.BusinessRules.RelationshipRules
{
	using System.Linq;

	using Crm.Library.BaseModel;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Validation;

	[Rule]
	public class RelationshipMustBeUnique : Rule<RelationshipBase>
	{
		private readonly IRepository<RelationshipBase> repository;
		public RelationshipMustBeUnique(IRepository<RelationshipBase> repository)
			: base(RuleClass.Unique)
		{
			this.repository = repository;
		}
		protected override RuleViolation CreateRuleViolation(RelationshipBase entity)
		{
			return RuleViolation(entity, x => x.ChildId, propertyNameReplacementKey: "Relationship");
		}
		protected override bool IsIgnoredFor(RelationshipBase entity)
		{
			return entity is LookupRelationshipBase;
		}
		public override bool IsSatisfiedBy(RelationshipBase entity)
		{
			var conflictingRelationships = repository.GetAll()
				.Where(x => x.ParentId == entity.ParentId && x.ChildId == entity.ChildId && x.Id != entity.Id);
			if (conflictingRelationships.Any())
			{
				return false;
			}
			return true;
		}
	}
}
