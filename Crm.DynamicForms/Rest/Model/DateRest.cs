namespace Crm.DynamicForms.Rest.Model
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Date))]
	public class DateRest : DynamicFormElementRest, IDynamicFormInputElementRest
	{
		public bool Required { get; set; }
		[RestrictedField, NotReceived]
		public string DefaultResponseValue { get; set; }
	}
}
