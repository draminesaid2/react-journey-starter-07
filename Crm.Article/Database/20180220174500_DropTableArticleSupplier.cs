namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180220174500)]
	public class DropTableArticleSupplier : Migration
	{
		public override void Up()
		{
			Database.DropTableIfExistingAndEmpty("CRM", "ArticleSupplier");
		}
	}
}