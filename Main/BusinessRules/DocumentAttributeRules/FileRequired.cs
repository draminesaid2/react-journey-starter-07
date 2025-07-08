namespace Crm.BusinessRules.DocumentAttributeRules
{
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;
	using Crm.Model;

	public class FileRequiredRule : RequiredRule<DocumentAttribute>
	{
		public FileRequiredRule()
		{
			Init(x => x.FileResource);
		}
		protected override RuleViolation CreateRuleViolation(DocumentAttribute entity)
		{
			return RuleViolation(entity, x => x.FileResource, errorMessageKey: "PleaseSelectFile");
		}
		public override bool IsSatisfiedBy(DocumentAttribute entity)
		{
			return entity.FileResourceKey.HasValue || base.IsSatisfiedBy(entity);
		}
	}
}