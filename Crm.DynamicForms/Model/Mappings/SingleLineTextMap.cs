namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class SingleLineTextMap : SubclassMapping<SingleLineText>
	{
		public SingleLineTextMap()
		{
			DiscriminatorValue(SingleLineText.DiscriminatorValue);

			Property(x => x.Required);
			Property(x => x.RowSize);

			Property(x => x.MinLength, m => m.Column("Min"));
			Property(x => x.MaxLength, m => m.Column("Max"));
		}
	}
}