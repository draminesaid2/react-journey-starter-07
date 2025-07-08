using Crm.DynamicForms.Model;
using Crm.Library.BaseModel;
using Crm.Library.Rest;
using Crm.Library.Rest.Interfaces;
using System;

namespace Crm.DynamicForms.Rest.Model
{
	[RestTypeFor(DomainType = typeof(DynamicFormFileResponse))]
	public class DynamicFormFileResponseRest : RestEntity, IRestEntityWithExtensionValues
	{
		public Guid Id { get; set; }
		public Guid DynamicFormReferenceKey { get; set; }
		public string LanguageKey { get; set; }
		public Guid FileResourceId { get; set; }

		public SerializableDictionary<string, object> ExtensionValues { get; set; }
	}
}
