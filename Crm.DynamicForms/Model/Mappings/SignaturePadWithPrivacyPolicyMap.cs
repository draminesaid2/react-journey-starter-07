namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class SignaturePadWithPrivacyPolicyMap : SubclassMapping<SignaturePadWithPrivacyPolicy>
	{
		public SignaturePadWithPrivacyPolicyMap()
		{
			DiscriminatorValue(SignaturePadWithPrivacyPolicy.DiscriminatorValue);

			Property(x => x.Required);
		}
	}
}