namespace Crm.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;

	public abstract class ContactRest : RestEntityWithExtensionValues
	{
		public virtual string Name { get; set; }
		public virtual Guid? ParentId { get; set; }
		[NotReceived, RestrictedField] public virtual string ParentName { get; set; }
		[NotReceived, RestrictedField(Restriction.NotSortable)] public virtual string ParentType { get; set; }
		public virtual string LegacyId { get; set; }
		[NotReceived] public virtual string LegacyName { get; set; }
		public virtual string LanguageKey { get; set; }
		public virtual string BackgroundInfo { get; set; }
		public virtual Guid? CampaignSource { get; set; }
		public virtual string ResponsibleUser { get; set; }
		public virtual string SourceTypeKey { get; set; }
		[NotReceived, ExplicitExpand] public NoteRest[] Notes { get; set; }
		public virtual Visibility Visibility { get; set; }
		[RestrictedField] public virtual Guid[] VisibleToUsergroupIds { get; set; }
		[RestrictedField] public virtual string[] VisibleToUserIds { get; set; }
	}
}
