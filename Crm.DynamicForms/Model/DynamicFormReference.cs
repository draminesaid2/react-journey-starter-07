namespace Crm.DynamicForms.Model
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	using Crm.DynamicForms.Model.Extensions;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Extensions;

	using Newtonsoft.Json;

	public class DynamicFormReference : EntityBase<Guid>, ISoftDelete
	{
		public virtual string LegacyId { get; set; }
		public virtual string ReferenceType { get; set; }
		public virtual Guid? ReferenceKey { get; set; }
		public virtual Guid DynamicFormKey { get; set; }
		[JsonIgnore]
		public virtual DynamicForm DynamicForm { get; set; }
		public virtual bool Completed { get; set; }
		public virtual ICollection<DynamicFormResponse> Responses { get; set; } = new List<DynamicFormResponse>();
		[Obsolete("must be moved to an extension or service")]
		public virtual string Filename
		{
			get { return DynamicForm != null && DynamicForm.GetTitle() != null ? DynamicForm.GetTitle().Replace(Path.GetInvalidFileNameChars(), '_') : Id.ToString(); }
		}
		[Obsolete("use dynamic form localized title instead")]
		public virtual string DynamicFormTitle
		{
			get { return DynamicForm != null ? DynamicForm.GetTitle() : String.Empty; }
		}
	}
}