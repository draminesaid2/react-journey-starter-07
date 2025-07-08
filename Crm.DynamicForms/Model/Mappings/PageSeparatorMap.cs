namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class PageSeparatorMap : SubclassMapping<PageSeparator>
	{
		public PageSeparatorMap()
		{
			DiscriminatorValue(PageSeparator.DiscriminatorValue);
		}
	}
}