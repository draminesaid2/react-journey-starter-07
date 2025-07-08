namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class SectionSeparatorMap : SubclassMapping<SectionSeparator>
	{
		public SectionSeparatorMap()
		{
			DiscriminatorValue(SectionSeparator.DiscriminatorValue);
		}
	}
}