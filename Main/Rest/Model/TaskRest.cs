namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Crm.Model.Task))]
	public class TaskRest : RestEntityWithExtensionValues
	{
		public Guid? ContactId { get; set; }
		[NotReceived] public string ContactType { get; set; }
		[NotReceived] public string ContactName { get; set; }
		public bool IsCompleted { get; set; }
		[NotReceived] public string LegacyId { get; set; }
		public string Text { get; set; }
		public string TypeKey { get; set; }
		public DateTime? DueDate { get; set; }
		public DateTime? DueTime { get; set; }
		public string ResponsibleUser { get; set; }
		[NotReceived, ExplicitExpand] public UserRest ResponsibleUserUser { get; set; }
		public Guid? ResponsibleUserGroupKey { get; set; }
		[NotReceived] public string ResponsibleUserGroup { get; set; }
		[RestrictedField, NotReceived] public Guid? TaskCampaignKey { get; set; }
		public string TaskCreateUser { get; set; }
		}
}
