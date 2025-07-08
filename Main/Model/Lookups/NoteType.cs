namespace Crm.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[NoteType]")]
	public class NoteType : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; } = "#00bcd4";
		[LookupProperty(Shared = true)]
		public virtual string Icon { get; set; } = "\\f25c";
		[LookupProperty(Shared = true)]
		public virtual bool IsSyncable { get; set; }
	}
}
