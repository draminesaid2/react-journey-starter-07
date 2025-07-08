namespace Crm.DynamicForms.Database
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	[Migration(20180606135500)]
	public class ChangeCrmDynamicFormToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_DynamicFormLanguage_DynamicForm') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormLanguage' AND COLUMN_NAME='DynamicFormKey' AND DATA_TYPE like '%int')
					BEGIN
						ALTER TABLE [CRM].[DynamicFormLanguage] DROP CONSTRAINT [FK_DynamicFormLanguage_DynamicForm]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_DynamicFormElement_DynamicForm') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormElement' AND COLUMN_NAME='DynamicFormKey' AND DATA_TYPE like '%int')
					BEGIN
						ALTER TABLE [CRM].[DynamicFormElement] DROP CONSTRAINT [FK_DynamicFormElement_DynamicForm]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_DynamicFormLocalization_DynamicForm') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='LU' AND TABLE_NAME='DynamicFormLocalization' AND COLUMN_NAME='DynamicFormId' AND DATA_TYPE like '%int')
					BEGIN
						ALTER TABLE [LU].[DynamicFormLocalization] DROP CONSTRAINT [FK_DynamicFormLocalization_DynamicForm]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_DynamicFormReference_DynamicForm') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormReference' AND COLUMN_NAME='DynamicFormKey' AND DATA_TYPE like '%int')
					BEGIN
						ALTER TABLE [CRM].[DynamicFormReference] DROP CONSTRAINT [FK_DynamicFormReference_DynamicForm]
					END

			IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ChecklistInstallationTypeRelationship_DynamicForm') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ChecklistInstallationTypeRelationship' AND COLUMN_NAME='DynamicFormKey' AND DATA_TYPE like '%int')
					BEGIN
						ALTER TABLE [SMS].[ChecklistInstallationTypeRelationship] DROP CONSTRAINT [FK_ChecklistInstallationTypeRelationship_DynamicForm]
					END");

			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicForm' AND COLUMN_NAME='DynamicFormId' AND DATA_TYPE like '%int'") == 1)
			{
				Database.RemovePrimaryKey("CRM", "DynamicForm");
			}

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicForm' AND COLUMN_NAME='DynamicFormId' AND DATA_TYPE like '%int')
				BEGIN
					ALTER TABLE [CRM].[DynamicForm] ADD [DynamicFormIdOld] int NULL
					EXEC('UPDATE [CRM].[DynamicForm] SET [DynamicFormIdOld] = [DynamicFormId]')
					ALTER TABLE [CRM].[DynamicForm] DROP COLUMN [DynamicFormId]
					ALTER TABLE [CRM].[DynamicForm] ADD [DynamicFormId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [CRM].[DynamicForm] ADD CONSTRAINT [PK_DynamicForm] PRIMARY KEY CLUSTERED ([DynamicFormId] ASC)
				END");
			
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormElement' AND COLUMN_NAME='DynamicFormKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[CRM].[DynamicFormElement].[DynamicFormKey]', 'DynamicFormKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[DynamicFormElement] ADD [DynamicFormKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DynamicFormKey] = b.[DynamicFormId] FROM [CRM].[DynamicFormElement] a LEFT OUTER JOIN [CRM].[DynamicForm] b ON a.[DynamicFormKeyOld] = b.[DynamicFormIdOld]')
					DELETE FROM [CRM].[DynamicFormElement] WHERE [DynamicFormKey] IS NULL
					ALTER TABLE [CRM].[DynamicFormElement] ALTER COLUMN [DynamicFormKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[DynamicFormElement] ALTER COLUMN [DynamicFormKeyOld] int NULL
					ALTER TABLE [CRM].[DynamicFormElement] ADD CONSTRAINT [FK_DynamicFormElement_DynamicForm] FOREIGN KEY([DynamicFormKey]) REFERENCES [CRM].[DynamicForm]([DynamicFormId]) ON UPDATE CASCADE ON DELETE CASCADE
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormLanguage' AND COLUMN_NAME='DynamicFormKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[CRM].[DynamicFormLanguage].[DynamicFormKey]', 'DynamicFormKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[DynamicFormLanguage] ADD [DynamicFormKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DynamicFormKey] = b.[DynamicFormId] FROM [CRM].[DynamicFormLanguage] a LEFT OUTER JOIN [CRM].[DynamicForm] b ON a.[DynamicFormKeyOld] = b.[DynamicFormIdOld]')
					DELETE FROM [CRM].[DynamicFormLanguage] WHERE [DynamicFormKey] IS NULL
					ALTER TABLE [CRM].[DynamicFormLanguage] ALTER COLUMN [DynamicFormKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[DynamicFormLanguage] ALTER COLUMN [DynamicFormKeyOld] int NULL
					ALTER TABLE [CRM].[DynamicFormLanguage] ADD CONSTRAINT [FK_DynamicFormLanguage_DynamicForm] FOREIGN KEY([DynamicFormKey]) REFERENCES [CRM].[DynamicForm]([DynamicFormId]) ON UPDATE CASCADE ON DELETE CASCADE
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='LU' AND TABLE_NAME='DynamicFormLocalization' AND COLUMN_NAME='DynamicFormId' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[LU].[DynamicFormLocalization].[DynamicFormId]', 'DynamicFormIdOld', 'COLUMN'
					ALTER TABLE [LU].[DynamicFormLocalization] ADD [DynamicFormId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DynamicFormId] = b.[DynamicFormId] FROM [LU].[DynamicFormLocalization] a LEFT OUTER JOIN [CRM].[DynamicForm] b ON a.[DynamicFormIdOld] = b.[DynamicFormIdOld]')
					DELETE FROM [LU].[DynamicFormLocalization] WHERE [DynamicFormId] IS NULL
					ALTER TABLE [LU].[DynamicFormLocalization] ALTER COLUMN [DynamicFormId] uniqueidentifier NOT NULL
					ALTER TABLE [LU].[DynamicFormLocalization] ALTER COLUMN [DynamicFormIdOld] int NULL
					ALTER TABLE [LU].[DynamicFormLocalization] ADD CONSTRAINT [FK_DynamicFormLocalization_DynamicForm] FOREIGN KEY([DynamicFormId]) REFERENCES [CRM].[DynamicForm]([DynamicFormId]) ON UPDATE CASCADE ON DELETE CASCADE
				END");
			
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormReference' AND COLUMN_NAME='DynamicFormKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[CRM].[DynamicFormReference].[DynamicFormKey]', 'DynamicFormKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[DynamicFormReference] ADD [DynamicFormKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DynamicFormKey] = b.[DynamicFormId] FROM [CRM].[DynamicFormReference] a LEFT OUTER JOIN [CRM].[DynamicForm] b ON a.[DynamicFormKeyOld] = b.[DynamicFormIdOld]')
					DELETE FROM [CRM].[DynamicFormReference] WHERE [DynamicFormKey] IS NULL
					ALTER TABLE [CRM].[DynamicFormReference] ALTER COLUMN [DynamicFormKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[DynamicFormReference] ALTER COLUMN [DynamicFormKeyOld] int NULL
					ALTER TABLE [CRM].[DynamicFormReference] ADD CONSTRAINT [FK_DynamicFormReference_DynamicForm] FOREIGN KEY([DynamicFormKey]) REFERENCES [CRM].[DynamicForm]([DynamicFormId]) ON UPDATE CASCADE ON DELETE CASCADE
				END");
			
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='SMS' AND TABLE_NAME='ChecklistInstallationTypeRelationship' AND COLUMN_NAME='DynamicFormKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[SMS].[ChecklistInstallationTypeRelationship].[DynamicFormKey]', 'DynamicFormKeyOld', 'COLUMN'
					ALTER TABLE [SMS].[ChecklistInstallationTypeRelationship] ADD [DynamicFormKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DynamicFormKey] = b.[DynamicFormId] FROM [SMS].[ChecklistInstallationTypeRelationship] a LEFT OUTER JOIN [CRM].[DynamicForm] b ON a.[DynamicFormKeyOld] = b.[DynamicFormIdOld]')
					DELETE FROM [SMS].[ChecklistInstallationTypeRelationship] WHERE [DynamicFormKey] IS NULL
					ALTER TABLE [SMS].[ChecklistInstallationTypeRelationship] ALTER COLUMN [DynamicFormKey] uniqueidentifier NOT NULL
					ALTER TABLE [SMS].[ChecklistInstallationTypeRelationship] ALTER COLUMN [DynamicFormKeyOld] int NULL
					ALTER TABLE [SMS].[ChecklistInstallationTypeRelationship] ADD CONSTRAINT [FK_ChecklistInstallationTypeRelationship_DynamicForm] FOREIGN KEY([DynamicFormKey]) REFERENCES [CRM].[DynamicForm]([DynamicFormId]) ON UPDATE CASCADE ON DELETE CASCADE
				END");

			var helper = new UnicoreMigrationHelper(Database);
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<DynamicForm>("CRM", "DynamicForm");
		}
	}
}