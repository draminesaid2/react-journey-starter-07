namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160629120000)]
	public class AlterArticleDescriptionAddConstraints : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				if not exists (
						select *
							from sys.all_columns c
							join sys.tables t on t.object_id = c.object_id
							join sys.schemas s on s.schema_id = t.schema_id
							join sys.default_constraints d on c.default_object_id = d.object_id
						where s.name = 'LU'
						and t.name = 'ArticleDescription'
							and c.name = 'CreateDate')
				BEGIN
					ALTER table [LU].[ArticleDescription] ADD CONSTRAINT DF_ArticleDescription_CreateDate DEFAULT GETUTCDATE() FOR [CreateDate];
				END;

				UPDATE [LU].[ArticleDescription] SET [CreateDate] = GETUTCDATE() WHERE [CreateDate] IS NULL;
				ALTER TABLE [LU].[ArticleDescription] ALTER COLUMN [CreateDate] DATETIME NOT NULL;

				if not exists (
						select *
							from sys.all_columns c
							join sys.tables t on t.object_id = c.object_id
							join sys.schemas s on s.schema_id = t.schema_id
							join sys.default_constraints d on c.default_object_id = d.object_id
						where s.name = 'LU'
						and t.name = 'ArticleDescription'
							and c.name = 'ModifyDate')
				BEGIN
					ALTER table [LU].[ArticleDescription] ADD CONSTRAINT DF_ArticleDescription_ModifyDate DEFAULT GETUTCDATE() FOR [ModifyDate];
				END;

				UPDATE [LU].[ArticleDescription] SET [ModifyDate] = GETUTCDATE() WHERE [ModifyDate] IS NULL;
				ALTER TABLE [LU].[ArticleDescription] ALTER COLUMN [ModifyDate] DATETIME NOT NULL;

				if not exists (
						select *
							from sys.all_columns c
							join sys.tables t on t.object_id = c.object_id
							join sys.schemas s on s.schema_id = t.schema_id
							join sys.default_constraints d on c.default_object_id = d.object_id
						where s.name = 'LU'
						and t.name = 'ArticleDescription'
							and c.name = 'IsActive')
				BEGIN
					ALTER table [LU].[ArticleDescription] ADD CONSTRAINT DF_ArticleDescription_IsActive DEFAULT 1 FOR [IsActive];
				END;

				UPDATE [LU].[ArticleDescription] SET [IsActive] = 1 WHERE [IsActive] IS NULL;
				ALTER TABLE [LU].[ArticleDescription] ALTER COLUMN [IsActive] BIT NOT NULL;
			");
		}
		public override void Down()
		{
		}
	}
}