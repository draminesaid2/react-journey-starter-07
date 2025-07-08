namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class TimeMap : SubclassMapping<Time>
	{
		public TimeMap()
		{
			DiscriminatorValue(Time.DiscriminatorValue);

			Property(x => x.Required);
		}
	}
}