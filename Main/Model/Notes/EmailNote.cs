namespace Crm.Model.Notes
{
	using Crm.Library.Extensions;

	public class EmailNote : Note, INoteWithSubject
	{
		public override string ImageVirtualUrl
		{
			get { return "~/Content/img/{0}.gif".WithArgs(ContactType); }
		}
		public override string PermanentLabelResourceKey
		{
			get { return "EmailBy"; }
		}
		public EmailNote()
		{
			IsSystemGenerated = false;
		}
	}
}