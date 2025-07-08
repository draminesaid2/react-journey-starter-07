namespace Crm.Model.Mappings.Notes
{
	using Crm.Model.Notes;

	using NHibernate.Mapping.ByCode.Conformist;

	public class EmailNoteMap : SubclassMapping<EmailNote>
	{
		public EmailNoteMap()
		{
			DiscriminatorValue("EmailNote");
		}
	}
}