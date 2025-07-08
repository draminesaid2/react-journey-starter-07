namespace Crm.Article.Model.Enums
{
	public enum DiscountType
	{
		NoDiscount = 0,
		Percentage = 1,
		Absolute = 2
	}

	public static class DiscountTypeExtensions
	{
		public static string GetSuffix(this DiscountType dt)
		{
			switch (dt)
			{
				case DiscountType.Percentage:
					return "%";
				default:
					return "";
			}
		}
	}
}