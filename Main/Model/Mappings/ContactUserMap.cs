namespace Crm.Model.Mappings
{
	using Crm.Model;

	using NHibernate.Mapping.ByCode.Conformist;

	public class ContactUserMap : ClassMapping<ContactUser>
	{
		public ContactUserMap()
		{
			Schema("CRM");
			Table("ContactUser");
			ComposedId(c =>
			{
				c.Property(x => x.UserName);
				c.Property(x => x.ContactKey);
			});
			Property(x => x.UserName);
			Property(x => x.ContactKey);
		}
	}
}