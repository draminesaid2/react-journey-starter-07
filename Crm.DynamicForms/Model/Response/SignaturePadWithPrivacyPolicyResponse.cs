namespace Crm.DynamicForms.Model.Response
{
	using System;

	[Serializable]
	public class SignaturePadWithPrivacyPolicyResponse
	{
		public virtual int DynamicFormResponseKey { get; set; }

		public virtual string Signature { get; set; }
		public virtual bool AcceptedPrivacyPolicy { get; set; }
	}
}