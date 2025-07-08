namespace Crm.Article.Model.Lookups
{
	using Library.Globalization.Lookup;

	[Lookup("[LU].[VATLevel]", "VATLevelId")]
	public class VATLevel : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual decimal PercentageValue { get; set; }
		[LookupProperty(Shared = true)]
		public virtual string CountryKey { get; set; }
	}
}
