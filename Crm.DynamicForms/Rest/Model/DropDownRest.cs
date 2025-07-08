namespace Crm.DynamicForms.Rest.Model
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(DropDown))]
	public class DropDownRest : DynamicFormElementRest, IDynamicFormInputElementRest
	{
		public bool Required { get; set; }
		public int Choices { get; set; }
		public bool Randomized { get; set; }
		public int RowSize { get; set; }
		[RestrictedField, NotReceived]
		public string DefaultResponseValue { get; set; }
	}
}
