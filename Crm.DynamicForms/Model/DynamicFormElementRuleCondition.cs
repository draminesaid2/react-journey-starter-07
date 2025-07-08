namespace Crm.DynamicForms.Model
{
	using System;

	using Crm.DynamicForms.Model.BaseModel;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class DynamicFormElementRuleCondition : EntityBase<Guid>, INoAuthorisedObject, ISoftDelete
	{
		public virtual DynamicFormElement DynamicFormElement { get; set; }
		public virtual Guid DynamicFormElementId { get; set; }
		public virtual DynamicFormElementRule DynamicFormElementRule { get; set; }
		public virtual Guid DynamicFormElementRuleId { get; set; }
		public virtual string Filter { get; set; }
		public virtual string Value { get; set; }
	}
}
