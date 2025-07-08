namespace Crm.DynamicForms.Rest.Model
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Time))]
	public class TimeRest : DynamicFormElementRest, IDynamicFormInputElementRest
	{
		public bool Required { get; set; }
		[RestrictedField, NotReceived]
		public string DefaultResponseValue { get; set; }
	}
}
