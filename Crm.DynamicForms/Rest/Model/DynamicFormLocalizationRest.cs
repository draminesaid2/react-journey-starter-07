namespace Crm.DynamicForms.Rest.Model
{
	using System;

	using Crm.DynamicForms.Model.Lookups;
	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;

	[RestTypeFor(DomainType = typeof(DynamicFormLocalization))]
	public class DynamicFormLocalizationRest : RestEntity, IRestEntityWithExtensionValues
	{
		public int? ChoiceIndex { get; set; }
		public Guid? DynamicFormElementId { get; set; }
		public Guid DynamicFormId { get; set; }
		public bool Favorite { get; set; }
		public string Hint { get; set; }
		public int Id { get; set; }
		[RestrictedField]
		public string Key { get; set; }
		public string Language { get; set; }
		public int SortOrder { get; set; }
		public string Value { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
	}
}
