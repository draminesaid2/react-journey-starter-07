namespace Crm.DynamicForms.Model
{
	using System;

	using Crm.DynamicForms.Model.Lookups;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Globalization;

	public class DynamicFormLanguage : EntityBase<Guid>, ISoftDelete
	{
		public virtual Guid DynamicFormKey { get; set; }
		public virtual DynamicForm DynamicForm { get; set; }
		public virtual string LanguageKey { get; set; }
		public virtual Language Language => LanguageKey != null ? LookupManager.Get<Language>(LanguageKey) : null;
		public virtual string StatusKey { get; set; }
		public virtual DynamicFormStatus Status => StatusKey != null ? LookupManager.Get<DynamicFormStatus>(StatusKey) : null;
		public virtual Guid? FileResourceId { get; set; }
	}
}
