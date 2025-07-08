namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class NumberMap : SubclassMapping<Number>
	{
		public NumberMap()
		{
			DiscriminatorValue(Number.DiscriminatorValue);

			Property(x => x.Required);

			Property(x => x.MinValue, m => m.Column("Min"));
			Property(x => x.MaxValue, m => m.Column("Max"));
			Property(x => x.RowSize);
		}
	}
}