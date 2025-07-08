namespace Crm.Article.Database
{
	using Crm.Article.Model;
	using Crm.Article.Model.Relationships;
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	[Migration(20180220180000)]
	public class AddEntityType : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<ArticleRelationship>("CRM", "ArticleRelationship");
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<Article>("CRM", "Contact");
		}
	}
}