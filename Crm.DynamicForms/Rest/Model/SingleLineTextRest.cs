namespace Crm.DynamicForms.Rest.Model
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(SingleLineText))]
	public class SingleLineTextRest : DynamicFormElementRest, IDynamicFormInputElementRest
	{
		public int MinLength { get; set; }
		public int MaxLength { get; set; }
		public bool Required { get; set; }
		public int RowSize { get; set; }
		[RestrictedField, NotReceived]
		public string DefaultResponseValue { get; set; }
	}
}
