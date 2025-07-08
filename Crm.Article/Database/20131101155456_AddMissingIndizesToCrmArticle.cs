namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131101155456)]
	public class AddMissingIndizesToCrmArticle : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Article]') AND name = 'IX_ArticleType')
                                BEGIN
	                                DROP INDEX [IX_ArticleType] ON [CRM].[Article]
                                END
	                              CREATE NONCLUSTERED INDEX IX_ArticleType ON [CRM].[Article] ([ArticleType]) INCLUDE ([ArticleId])");
		}

		public override void Down()
		{
		}
	}
}