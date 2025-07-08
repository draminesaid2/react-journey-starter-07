namespace Crm.DynamicForms.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.DynamicForms.Model.BaseModel;
	using Crm.DynamicForms.Model.Enums;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class DynamicFormElementRule : EntityBase<Guid>, INoAuthorisedObject, ISoftDelete
	{
		public virtual ICollection<DynamicFormElementRuleCondition> Conditions { get; set; }
		public virtual DynamicForm DynamicForm { get; set; }
		public virtual Guid DynamicFormId { get; set; }
		public virtual DynamicFormElement DynamicFormElement { get; set; }
		public virtual Guid DynamicFormElementId { get; set; }
		public virtual DynamicFormElementRuleMatchType MatchType { get; set; }
		public virtual DynamicFormElementRuleType Type { get; set; }
	}
}
