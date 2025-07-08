namespace Crm.DynamicForms.Model.Mappings
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class LiteralMap : SubclassMapping<Literal>
	{
		public LiteralMap()
		{
			DiscriminatorValue(Literal.DiscriminatorValue);
		}
	}
}