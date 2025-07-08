namespace Crm.DynamicForms.BusinessRules.DynamicForm
{
	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;

	public class TitleRequired : RequiredRule<DynamicFormLocalization>
	{
		public TitleRequired()
		{
			Init(x => x.Value, nameof(DynamicForm.Title));
		}
		public override RuleViolation GetRuleViolation(object entity)
		{
			var result = base.GetRuleViolation(entity);
			if (result != null)
			{
				result.DisplayRegion = nameof(DynamicForm.Title);
			}

			return result;
		}
		protected override bool IsIgnoredFor(DynamicFormLocalization entity)
		{
			return entity.DynamicFormElementId.HasValue;
		}
	}
}
