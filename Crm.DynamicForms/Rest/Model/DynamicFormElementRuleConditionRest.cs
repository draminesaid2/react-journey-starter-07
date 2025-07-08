namespace Crm.DynamicForms.Rest.Model
{
	using System;

	using Crm.DynamicForms.Model;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(DynamicFormElementRuleCondition))]
	public class DynamicFormElementRuleConditionRest : RestEntity
	{
		public Guid Id { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
		public Guid DynamicFormElementId { get; set; }
		public Guid DynamicFormElementRuleId { get; set; }
		public string Filter { get; set; }
		public string Value { get; set; }
	}
}
