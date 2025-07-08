namespace Crm.DynamicForms.Rest.Model
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(CheckBoxList))]
	public class CheckBoxListRest : DynamicFormElementRest, IDynamicFormInputElementRest
	{
		public int MinChoices { get; set; }
		public int MaxChoices { get; set; }

		public bool Required { get; set; }

		public int Choices { get; set; }
		public bool Randomized { get; set; }
		public int Layout { get; set; }
		[RestrictedField, NotReceived]
		public string DefaultResponseValue
		{
			get => "[]";
			set { }
		}
	}
}
