namespace Crm.DynamicForms.Rest.Model
{
	using System;

	using Crm.DynamicForms.Model.BaseModel;
	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;

	[RestTypeFor(DomainType = typeof(DynamicFormElement)), ExplicitEntitySet]
	public abstract class DynamicFormElementRest : RestEntity, IRestEntityWithExtensionValues
	{
		public Guid Id { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }

		public string LegacyId { get; set; }
		public Guid DynamicFormKey { get; set; }
		public string FormElementType { get; set; }
		[ExplicitExpand, NotReceived] public DynamicFormLocalizationRest[] Localizations { get; set; }
		[ExplicitExpand, NotReceived] public DynamicFormElementRuleRest[] Rules { get; set; }
		public int SortOrder { get; set; }
		public int Size { get; set; }
		public string CssExtra { get; set; }
	}
}
