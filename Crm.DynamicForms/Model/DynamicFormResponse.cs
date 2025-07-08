namespace Crm.DynamicForms.Model
{
	using System;

	using Crm.DynamicForms.Model.BaseModel;
	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class DynamicFormResponse : EntityBase<Guid>, ISoftDelete, INoAuthorisedObject
	{
		public virtual Guid DynamicFormReferenceKey { get; set; }
		public virtual DynamicFormElement DynamicFormElement { get; set; }
		public virtual Guid DynamicFormElementKey { get; set; }
		public virtual Guid DynamicFormKey => DynamicFormElement.DynamicFormKey;
		public virtual string DynamicFormElementType { get; set; }
		public virtual string ValueAsString { get; set; }
	}
}
