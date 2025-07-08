namespace Crm.DynamicForms.Model
{
	using Crm.DynamicForms.Model.BaseModel;
	using Crm.DynamicForms.Model.Response;

	using Newtonsoft.Json;

	public class SignaturePadWithPrivacyPolicy : DynamicFormElement, IDynamicFormInputElement<SignaturePadWithPrivacyPolicyResponse>
	{
		public static string DiscriminatorValue = "SignaturePadWithPrivacyPolicy";
		public virtual bool Required { get; set; }
		public virtual SignaturePadWithPrivacyPolicyResponse Response { get; set; }

		public override string ParseFromClient(string value)
		{
			if (value == null)
			{
				return null;
			}
			var response = JsonConvert.DeserializeObject<SignaturePadWithPrivacyPolicyResponse>(value);
			return JsonConvert.SerializeObject(response);
		}
		public override string ParseToClient(string value)
		{
			if (value == null)
			{
				return null;
			}
			var response = JsonConvert.DeserializeObject<SignaturePadWithPrivacyPolicyResponse>(value);
			return JsonConvert.SerializeObject(response);
		}

		public SignaturePadWithPrivacyPolicy()
		{
			Response = new SignaturePadWithPrivacyPolicyResponse();
			Size = 2;
		}
	}
}
