namespace Crm.DynamicForms.Rest.Model
{
	using System;

	using Crm.DynamicForms.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;

	[RestTypeFor(DomainType = typeof(DynamicFormResponse))]
	public class DynamicFormResponseRest : RestEntity, IRestEntityWithExtensionValues
	{
		public Guid Id { get; set; }
		public Guid DynamicFormReferenceKey { get; set; }
		[NotReceived]
		public Guid DynamicFormKey { get; set; }
		public Guid DynamicFormElementKey { get; set; }
		public string DynamicFormElementType { get; set; }
		[RestrictedField]
		public string Value { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
	}
}
