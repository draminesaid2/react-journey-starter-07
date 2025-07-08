namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class SignaturePadMap : SubclassMapping<SignaturePad>
	{
		public SignaturePadMap()
		{
			DiscriminatorValue(SignaturePad.DiscriminatorValue);

			Property(x => x.Required);
		}
	}
}