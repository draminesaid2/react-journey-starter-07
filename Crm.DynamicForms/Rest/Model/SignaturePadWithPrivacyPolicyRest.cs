namespace Crm.DynamicForms.Rest.Model
{
	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.Response;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	using Newtonsoft.Json;

	[RestTypeFor(DomainType = typeof(SignaturePadWithPrivacyPolicy))]
	public class SignaturePadWithPrivacyPolicyRest : DynamicFormElementRest, IDynamicFormInputElementRest
	{
		public bool Required { get; set; }
		private static readonly string defaultResponseValue = JsonConvert.SerializeObject(new SignaturePadWithPrivacyPolicyResponse());
		[RestrictedField, NotReceived]
		public string DefaultResponseValue
		{
			get => defaultResponseValue;
			set { }
		}
	}
}
