namespace Crm.DynamicForms.Rest.Model
{
	using System;

	using Crm.DynamicForms.Model;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;

	[RestTypeFor(DomainType = typeof(DynamicFormLanguage))]
	public class DynamicFormLanguageRest : RestEntity, IRestEntityWithExtensionValues
	{
		public Guid Id { get; set; }
		public Guid DynamicFormKey { get; set; }
		public string LanguageKey { get; set; }
		public string StatusKey { get; set; }
		public Guid? FileResourceId { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
	}
}
