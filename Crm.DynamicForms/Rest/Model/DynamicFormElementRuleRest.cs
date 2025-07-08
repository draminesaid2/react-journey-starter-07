namespace Crm.DynamicForms.Rest.Model
{
	using System;

	using Crm.DynamicForms.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(DynamicFormElementRule))]
	public class DynamicFormElementRuleRest : RestEntity
	{
		public Guid Id { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
		[ExplicitExpand, NotReceived] public DynamicFormElementRuleConditionRest[] Conditions { get; set; }
		public Guid DynamicFormId { get; set; }
		public Guid DynamicFormElementId { get; set; }
		[RestrictedField]
		public string MatchType { get; set; }
		[RestrictedField]
		public string Type { get; set; }
	}
}
