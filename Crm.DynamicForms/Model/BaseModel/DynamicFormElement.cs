namespace Crm.DynamicForms.Model.BaseModel
{
	using System;
	using System.Collections.Generic;

	using Crm.DynamicForms.Model.Lookups;
	using Crm.Library.Api.Authorization;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public abstract class DynamicFormElement : EntityBase<Guid>, IDynamicFormElement, ISoftDelete, INoAuthorisedObject, IGroupPermissionNameParent
	{
		public virtual string LegacyId { get; set; }
		public virtual Guid DynamicFormKey { get; set; }
		private string formElementType;
		public virtual ICollection<DynamicFormLocalization> Localizations { get; set; }
		public virtual ICollection<DynamicFormElementRule> Rules { get; set; } = new List<DynamicFormElementRule>();
		public virtual string FormElementType
		{
			get => formElementType ?? GetType().Name;
			set => formElementType = value;
		}
		public virtual int SortOrder { get; set; }
		public virtual int Size { get; set; }
		public virtual string CssExtra { get; set; }
		public DynamicFormElement()
		{
			Size = 1;
		}
		public virtual string ParseFromClient(string value)
		{
			return value.ToString();
		}
		public virtual string ParseToClient(string value)
		{
			return value;
		}
	}
}
