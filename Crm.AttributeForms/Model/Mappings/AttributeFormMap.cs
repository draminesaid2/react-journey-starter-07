namespace Crm.AttributeForms.Model.Mappings
{
	using Crm.AttributeForms.Model;

	using NHibernate.Mapping.ByCode;
	using NHibernate.Mapping.ByCode.Conformist;

	public class AttributeFormMap : SubclassMapping<AttributeForm>
	{
		public AttributeFormMap()
		{
			DiscriminatorValue("AttributeForm");

			ManyToOne(x => x.Contact, m =>
			{
				m.Column("ReferenceKey");
				m.Insert(false);
				m.Update(false);
				m.Fetch(FetchKind.Select);
				m.Lazy(LazyRelation.Proxy);
			});
		}
	}
}