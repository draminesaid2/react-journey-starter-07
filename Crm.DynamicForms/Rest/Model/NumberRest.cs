namespace Crm.DynamicForms.Rest.Model
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Number))]
	public class NumberRest : DynamicFormElementRest, IDynamicFormInputElementRest
	{
		public decimal MinValue { get; set; }
		public decimal MaxValue { get; set; }
		public bool Required { get; set; }
		public int RowSize { get; set; }
		[RestrictedField, NotReceived]
		public string DefaultResponseValue { get; set; }
	}
}
