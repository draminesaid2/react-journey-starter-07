namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230911124500)]
	public class IncreaseArticleGroupSizes : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[LU].[ArticleGroup2]"))
				Database.ExecuteNonQuery("ALTER TABLE [LU].[ArticleGroup2] ALTER COLUMN ArticleGroupId INT NOT NULL");
			if (Database.TableExists("[LU].[ArticleGroup3]"))
				Database.ExecuteNonQuery("ALTER TABLE [LU].[ArticleGroup3] ALTER COLUMN ArticleGroupId INT NOT NULL");
			if (Database.TableExists("[LU].[ArticleGroup4]"))
				Database.ExecuteNonQuery("ALTER TABLE [LU].[ArticleGroup4] ALTER COLUMN ArticleGroupId INT NOT NULL");
			if (Database.TableExists("[LU].[ArticleGroup5]"))
				Database.ExecuteNonQuery("ALTER TABLE [LU].[ArticleGroup5] ALTER COLUMN ArticleGroupId INT NOT NULL");
		}
	}
}
