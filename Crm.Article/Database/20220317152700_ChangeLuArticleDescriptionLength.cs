namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220317152700)]
	public class ChangeLuArticleDescriptionLength : Migration
	{
		public override void Up()
		{
			var articleDescriptionLength = (int)Database.ExecuteScalar(
				@"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = 'LU' AND
								TABLE_NAME = 'ArticleDescription' AND
								COLUMN_NAME = 'Name'");

			if (articleDescriptionLength < 450)
			{
				Database.ExecuteNonQuery("ALTER TABLE [LU].[ArticleDescription] ALTER COLUMN [Name] NVARCHAR(450) NOT NULL");
			}
		}
	}
}