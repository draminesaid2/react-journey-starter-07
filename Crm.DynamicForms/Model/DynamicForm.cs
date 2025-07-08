namespace Crm.DynamicForms.Model
{
	using System;
	using System.Collections.Generic;
	using Crm.DynamicForms.Model.BaseModel;
	using Crm.DynamicForms.Model.Extensions;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class DynamicForm : EntityBase<Guid>, ISoftDelete
	{
		public virtual string DefaultLanguageKey { get; set; }
		public virtual string Title => this.GetTitle();
		public virtual bool HideEmptyOptional { get; set; }
		public virtual string Description => this.GetDescription();
		public virtual IList<DynamicFormElement> Elements { get; set; }
		public virtual ICollection<DynamicFormLanguage> Languages { get; set; }
		public virtual ICollection<DynamicFormLocalization> Localizations { get; set; }

		public virtual string CategoryKey { get; set; }
		public virtual DynamicFormCategory Category
		{
			get
			{
				return CategoryKey != null ? LookupManager.Get<DynamicFormCategory>(CategoryKey) : null;
			}
		}

		public DynamicForm()
		{
			Elements = new List<DynamicFormElement>();
			Languages = new List<DynamicFormLanguage>();
			Localizations = new List<DynamicFormLocalization>();
		}
	}
}