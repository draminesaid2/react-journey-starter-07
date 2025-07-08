namespace Crm.Model.Mappings.Notes
{
	using NHibernate.Mapping.ByCode.Conformist;

	public class UserNoteMap : SubclassMapping<UserNote>
	{
		public UserNoteMap()
		{
			DiscriminatorValue("UserNote");
		}
	}
}
