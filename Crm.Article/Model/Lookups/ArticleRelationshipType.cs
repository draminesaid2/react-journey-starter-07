namespace Crm.Article.Model.Lookups
{
	using System.Collections.Generic;

	using Crm.Library.Globalization.Lookup;

	[Lookup]
	public class ArticleRelationshipType : EntityLookup<string>, ILookupWithInverse
	{
		public static string AccessoryKey = "Accessory";
		public static string SetKey = "Set";

		[LookupProperty(Shared = true)]
		public virtual List<string> ArticleTypes { get; set; }
		[LookupProperty(Shared = true)]
		public virtual bool HasQuantity { get; set; }
		[LookupProperty(Shared = false, Column = "InverseName")]
		public virtual string InverseValue { get; set; }
	}
}