namespace Crm.Article.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[ArticleType]")]
	public class ArticleType : EntityLookup<string>, ILookupWithColor
	{
		// Members
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; }
		public const string MaterialKey = "Material";
		public const string ServiceKey = "Service";
		public const string ToolKey = "Tool";
		public const string CostKey = "Cost";
	}
}