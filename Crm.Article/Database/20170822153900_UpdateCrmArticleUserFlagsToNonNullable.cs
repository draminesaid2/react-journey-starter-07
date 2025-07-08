namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170822153900)]
	public class UpdateCrmArticleUserFlagsToNonNullable : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
ALTER TABLE [CRM].[Article] ADD DEFAULT 0 FOR [UserFlag01]
UPDATE [CRM].[Article] SET [UserFlag01] = 0 WHERE [UserFlag01] IS NULL
ALTER TABLE [CRM].[Article] ALTER COLUMN [UserFlag01] BIT NOT NULL

ALTER TABLE [CRM].[Article] ADD DEFAULT 0 FOR [UserFlag02]
UPDATE [CRM].[Article] SET [UserFlag02] = 0 WHERE [UserFlag02] IS NULL
ALTER TABLE [CRM].[Article] ALTER COLUMN [UserFlag02] BIT NOT NULL

ALTER TABLE [CRM].[Article] ADD DEFAULT 0 FOR [UserFlag03]
UPDATE [CRM].[Article] SET [UserFlag03] = 0 WHERE [UserFlag03] IS NULL
ALTER TABLE [CRM].[Article] ALTER COLUMN [UserFlag03] BIT NOT NULL

ALTER TABLE [CRM].[Article] ADD DEFAULT 0 FOR [UserFlag04]
UPDATE [CRM].[Article] SET [UserFlag04] = 0 WHERE [UserFlag04] IS NULL
ALTER TABLE [CRM].[Article] ALTER COLUMN [UserFlag04] BIT NOT NULL

ALTER TABLE [CRM].[Article] ADD DEFAULT 0 FOR [UserFlag05]
UPDATE [CRM].[Article] SET [UserFlag05] = 0 WHERE [UserFlag05] IS NULL
ALTER TABLE [CRM].[Article] ALTER COLUMN [UserFlag05] BIT NOT NULL

ALTER TABLE [CRM].[Article] ADD DEFAULT 0 FOR [IsSparePart]
UPDATE [CRM].[Article] SET [IsSparePart] = 0 WHERE [IsSparePart] IS NULL
ALTER TABLE [CRM].[Article] ALTER COLUMN [IsSparePart] BIT NOT NULL
");
		}
	}
}