namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160706165600)]
	public class AddImageToArticleGroups : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[LU].[ArticleGroup]", "Image"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [LU].[ArticleGroup] ADD Image VARBINARY(MAX) NULL");
			}
			if (!Database.ColumnExists("[LU].[ArticleGroup2]", "Image"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [LU].[ArticleGroup2] ADD Image VARBINARY(MAX) NULL");
			}
			if (!Database.ColumnExists("[LU].[ArticleGroup3]", "Image"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [LU].[ArticleGroup3] ADD Image VARBINARY(MAX) NULL");
			}
			if (!Database.ColumnExists("[LU].[ArticleGroup4]", "Image"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [LU].[ArticleGroup4] ADD Image VARBINARY(MAX) NULL");
			}
			if (!Database.ColumnExists("[LU].[ArticleGroup5]", "Image"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [LU].[ArticleGroup5] ADD Image VARBINARY(MAX) NULL");
			}
		}
	}
}