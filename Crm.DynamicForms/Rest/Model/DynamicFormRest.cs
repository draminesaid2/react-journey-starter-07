namespace Crm.DynamicForms.Rest.Model
{
	using System.Collections.Generic;

	using Crm.DynamicForms.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(DynamicForm))]
	public class DynamicFormRest : RestEntityWithExtensionValues
	{
		public virtual string DefaultLanguageKey { get; set; }
		public virtual bool HideEmptyOptional { get; set; }
		public virtual string CategoryKey { get; set; }
		[ExplicitExpand, NotReceived] public virtual ICollection<DynamicFormElementRest> Elements { get; set; }
		[ExplicitExpand, NotReceived] public virtual ICollection<DynamicFormLanguageRest> Languages { get; set; }
		[ExplicitExpand, NotReceived] public virtual ICollection<DynamicFormLocalizationRest> Localizations { get; set; }
	}
}
