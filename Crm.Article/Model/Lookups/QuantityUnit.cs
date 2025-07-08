namespace Crm.Article.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[QuantityUnit]")]
	public class QuantityUnit : EntityLookup<string>
	{
		// Members
		public const string PiecesKey = "Stk";
	}
}