namespace Crm.Article.Model
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[ProductFamilyStatus]", "ProductFamilyStatusId")]
	public class ProductFamilyStatus : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; }

		public ProductFamilyStatus()
		{
			Color = "#AAAAAA";
		}
	}
}