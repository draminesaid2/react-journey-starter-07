namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class RadioButtonListMap : SubclassMapping<RadioButtonList>
	{
		public RadioButtonListMap()
		{
			DiscriminatorValue(RadioButtonList.DiscriminatorValue);

			Property(x => x.Required);
			Property(x => x.Choices);
			Property(x => x.Randomized);
			Property(x => x.Layout);
		}
	}
}