namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Model;
	using Crm.Model.Notes;

	[RestTypeFor(DomainType = typeof(EmailNote))]
	public class EmailNoteRest : NoteRest
	{
	}
	[RestTypeFor(DomainType = typeof(TaskCompletedNote))]
	public class TaskCompletedNoteRest : NoteRest
	{
	}
	[RestTypeFor(DomainType = typeof(UserNote))]
	public class UserNoteRest : NoteRest
	{
	}
	[RestTypeFor(DomainType = typeof(Note))]
	public class NoteRest : RestEntityWithExtensionValues
	{
		public Guid? ContactId { get; set; }
		[NotReceived]
		public string ContactName { get; set; }
		[NotReceived]
		public string ContactType { get; set; }
		[NotReceived] public string LegacyId { get; set; }
		[ExplicitExpand, NotReceived] public UserRest User { get; set; }
		public string NoteType { get; set; }
		public string Subject { get; set; }
		public string Text { get; set; }
		public string Title { get; set; }
		public string Meta { get; set; }
		public string Link { get; set; }
		public bool IsSystemGenerated { get; set; }
		public string Plugin { get; set; }
		public Guid? EntityId { get; set; }
		public string EntityName { get; set; }
		[ExplicitExpand, NotReceived] public LinkResourceRest[] Links { get; set; }
		[ExplicitExpand, NotReceived] public FileResourceRest[] Files { get; set; }
	}
}
