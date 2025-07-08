namespace Crm.DynamicForms.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup]
	[NotEditable]
	public class DynamicFormStatus : EntityLookup<string>
	{
		public const string DraftKey = "Draft";
		public const string ReleasedKey = "Released";
		public const string DisabledKey = "Disabled";
		
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; }

		public DynamicFormStatus()
		{
			Color = "#9E9E9E";
		}
	}
}