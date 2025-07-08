namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class CheckBoxListMap : SubclassMapping<CheckBoxList>
	{
		public CheckBoxListMap()
		{
			DiscriminatorValue(CheckBoxList.DiscriminatorValue);

			Property(x => x.Required);

			Property(x => x.Choices);

			Property(x => x.MinChoices, m => m.Column("Min"));
			Property(x => x.MaxChoices, m => m.Column("Max"));
			Property(x => x.Randomized);
			Property(x => x.Layout);
		}
	}
}