namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210115123900)]
	public class ChangeArticleDescriptionLength : Migration
	{
		public override void Up()
		{
			var contactNameLength = (int)Database.ExecuteScalar(
				@"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = 'CRM' AND
								TABLE_NAME = 'Contact' AND
								COLUMN_NAME = 'Name'");

			var articleDescriptionLength = (int)Database.ExecuteScalar(
				@"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = 'CRM' AND
								TABLE_NAME = 'Article' AND
								COLUMN_NAME = 'Description'");

			if (contactNameLength == 450 && articleDescriptionLength < 450)
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[Article] ALTER COLUMN [Description] NVARCHAR(450) NOT NULL");
			}
		}
	}
}