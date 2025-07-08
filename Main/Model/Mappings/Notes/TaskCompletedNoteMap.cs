namespace Crm.Model.Mappings.Notes
{
	using Crm.Model.Notes;

	using NHibernate.Mapping.ByCode.Conformist;

	public class TaskCompletedNoteMap : SubclassMapping<TaskCompletedNote>
	{
		public TaskCompletedNoteMap()
		{
			DiscriminatorValue("TaskCompletedNote");
		}
	}
}