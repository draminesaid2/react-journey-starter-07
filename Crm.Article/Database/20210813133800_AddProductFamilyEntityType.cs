namespace Crm.Article.Database
{
	using Crm.Article.Model;
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	[Migration(20210813133800)]
	public class AddProductFamilyEntityType : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<ProductFamily>("CRM", "Contact");
		}
	}
}