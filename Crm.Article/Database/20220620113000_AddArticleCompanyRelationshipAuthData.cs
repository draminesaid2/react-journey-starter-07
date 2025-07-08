namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	[Migration(20220620113000)]
	public class AddArticleCompanyRelationshipAuthData : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddOrUpdateEntityAuthDataColumn<Model.Relationships.ArticleCompanyRelationship>("CRM", "ArticleCompanyRelationship");
		}
	}
}
