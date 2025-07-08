namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413113900)]
	public class AddArticleParentFkToCrmArticleRelationship : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ArticleRelationship_ParentArticleKey_Article'") == 0)
			{
				Database.ExecuteNonQuery("DELETE ar FROM [CRM].[ArticleRelationship] ar LEFT OUTER JOIN [CRM].[Article] a ON ar.[ParentArticleKey] = a.[ArticleId] WHERE a.[ArticleId] IS NULL");
				Database.AddForeignKey("FK_ArticleRelationship_ParentArticleKey_Article", "[CRM].[ArticleRelationship]", "ParentArticleKey", "[CRM].[Article]", "ArticleId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}