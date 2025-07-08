namespace Crm.Model.Mappings
{
	using Crm.Model;

	using NHibernate.Mapping.ByCode.Conformist;

	public class ContactUserGroupMap : ClassMapping<ContactUserGroup>
	{
		public ContactUserGroupMap()
		{
			Schema("CRM");
			Table("ContactUserGroup");
			ComposedId(c =>
			{
				c.Property(x => x.UsergroupKey);
				c.Property(x => x.ContactKey);
			});
			Property(x => x.UsergroupKey);
			Property(x => x.ContactKey);
		}
	}
}