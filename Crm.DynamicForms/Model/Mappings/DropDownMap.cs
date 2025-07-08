namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class DropDownMap : SubclassMapping<DropDown>
	{
		public DropDownMap()
		{
			DiscriminatorValue(DropDown.DiscriminatorValue);

			Property(x => x.Required);
			Property(x => x.Randomized);
			Property(x => x.RowSize);

			Property(x => x.Choices);
		}
	}
}