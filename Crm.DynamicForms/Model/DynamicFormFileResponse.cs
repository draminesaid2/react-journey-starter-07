using Crm.Library.BaseModel;
using Crm.Library.BaseModel.Interfaces;
using Crm.Library.Globalization;

using System;

namespace Crm.DynamicForms.Model
{
	public class DynamicFormFileResponse : EntityBase<Guid>, ISoftDelete, INoAuthorisedObject
	{
		public virtual Guid DynamicFormReferenceKey { get; set; }
		public virtual DynamicFormReference DynamicFormReference { get; set; }
		public virtual string LanguageKey { get; set; }
		public virtual Language Language => LanguageKey != null ? LookupManager.Get<Language>(LanguageKey) : null;
		public virtual Guid FileResourceId { get; set; }

	}
}
