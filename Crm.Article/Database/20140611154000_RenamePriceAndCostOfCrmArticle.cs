namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140611154000)]
	public class RenamePriceAndCostOfCrmArticle : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[CRM].[Article]", "Price") && !Database.ColumnExists("[CRM].[Article]", "SalesPrice"))
			{
				Database.RenameColumn("[CRM].[Article]", "Price", "SalesPrice");
			}
			if (Database.ColumnExists("[CRM].[Article]", "Cost") && !Database.ColumnExists("[CRM].[Article]", "PurchasePrice"))
			{
				Database.RenameColumn("[CRM].[Article]", "Cost", "PurchasePrice");
			}
		}
	}
}