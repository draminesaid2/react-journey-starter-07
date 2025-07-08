namespace Crm.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170309183000)]
	public class ChangeArticleRelationshipIdToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='ArticleRelationship' AND COLUMN_NAME='ArticleRelationshipId' AND DATA_TYPE = 'int')
BEGIN
	DECLARE @pkName AS NVARCHAR(MAX)
	SET @pkName = (SELECT name FROM sys.objects WHERE type = 'PK' AND parent_object_id = object_id('CRM.ArticleRelationship'))
	EXEC('ALTER TABLE [CRM].[ArticleRelationship] DROP CONSTRAINT ' + @pkName)
	EXEC sp_rename 'CRM.ArticleRelationship.ArticleRelationshipId', 'ArticleRelationshipIdOld', 'COLUMN'; 
	
	ALTER TABLE [CRM].[ArticleRelationship] ADD [ArticleRelationshipId] uniqueidentifier NOT NULL DEFAULT(newid())
	ALTER TABLE [CRM].[ArticleRelationship] ADD CONSTRAINT PK_ArticleRelationship PRIMARY KEY (ArticleRelationshipId)
	UPDATE [CRM].[ArticleRelationship] SET ModifyDate = GETUTCDATE(), ModifyUser = 'Migration_20170309183000'
END
		");
		}
	}
}