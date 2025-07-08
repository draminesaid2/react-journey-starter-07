namespace Crm.DynamicForms.Model.Lookups
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Globalization.Lookup;

	[Lookup(IgnoreValidation = true)]
	[NotEditable]
	[IgnoreMissingLookups]
	[RestrictedType(TypeRestriction.None)]
	public class DynamicFormLocalization : EntityLookup<string>, ILookup
	{
		[LookupProperty(Shared = true)]
		public virtual int? ChoiceIndex { get; set; }
		[LookupProperty(Shared = true)]
		public virtual Guid? DynamicFormElementId { get; set; }
		[LookupProperty(Shared = true)]
		public virtual Guid DynamicFormId { get; set; }
		[LookupProperty(Shared = true)]
		public virtual string Hint { get; set; }
		public override string Key => Id.ToString();
		object ILookup.Key
		{
			get { return Id.ToString(); }
			set { /* ignore */ }
		}
	}
}
