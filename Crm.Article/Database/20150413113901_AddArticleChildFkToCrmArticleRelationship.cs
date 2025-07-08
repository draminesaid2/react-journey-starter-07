namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413113901)]
	public class AddArticleChildFkToCrmArticleRelationship : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ArticleRelationship_ChildArticleKey_Article'") == 0)
			{
				Database.ExecuteNonQuery("DELETE ar FROM [CRM].[ArticleRelationship] ar LEFT OUTER JOIN [CRM].[Article] a ON ar.[ChildArticleKey] = a.[ArticleId] WHERE a.[ArticleId] IS NULL");
				Database.AddForeignKey("FK_ArticleRelationship_ChildArticleKey_Article", "[CRM].[ArticleRelationship]", "ChildArticleKey", "[CRM].[Article]", "ArticleId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}