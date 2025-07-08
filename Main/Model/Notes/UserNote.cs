namespace Crm.Model
{
	using Crm.Library.Extensions;
	using Crm.Model.Notes;

	public class UserNote : Note
	{
		public override string ImageVirtualUrl
		{
			get { return "~/Content/img/{0}.gif".WithArgs(ContactType); }
		}
		public override string PermanentLabelResourceKey
		{
			get { return "NoteBy"; }
		}
		public UserNote()
		{
			IsSystemGenerated = false;
		}
	}
}