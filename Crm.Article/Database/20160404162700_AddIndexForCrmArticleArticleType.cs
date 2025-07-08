namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404162700)]
	public class AddIndexForCrmArticleArticleType : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Article]') AND name = N'IX_ArticleType'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_ArticleType] ON [CRM].[Article]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_ArticleType] ON [CRM].[Article] ([ArticleType] ASC) INCLUDE ([ArticleId])");
		}
	}
}