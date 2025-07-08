namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class DateMap : SubclassMapping<Date>
	{
		public DateMap()
		{
			DiscriminatorValue(Date.DiscriminatorValue);

			Property(x => x.Required);
		}
	}
}