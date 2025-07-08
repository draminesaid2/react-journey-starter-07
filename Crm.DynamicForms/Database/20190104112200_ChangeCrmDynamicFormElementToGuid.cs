namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190104112200)]
	public class ChangeCrmDynamicFormElementToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(
				@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_DynamicFormLocalization_DynamicFormElement') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='LU' AND TABLE_NAME='DynamicFormLocalization' AND COLUMN_NAME='DynamicFormElementId' AND DATA_TYPE like '%int')
					BEGIN
						ALTER TABLE [LU].[DynamicFormLocalization] DROP CONSTRAINT [FK_DynamicFormLocalization_DynamicFormElement]
					END
				
					IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_DynamicFormResponse_DynamicFormElement') AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormResponse' AND COLUMN_NAME='DynamicFormElementKey' AND DATA_TYPE like '%int')
					BEGIN
						ALTER TABLE [CRM].[DynamicFormResponse] DROP CONSTRAINT [FK_DynamicFormResponse_DynamicFormElement]
					END");

			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.objects WHERE NAME = 'PK_DynamicFormResponse' AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormResponse' AND COLUMN_NAME='DynamicFormElementKey' AND DATA_TYPE like '%int')") == 1)
			{
				Database.RemovePrimaryKey("CRM", "DynamicFormResponse");
			}

			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormElement' AND COLUMN_NAME='DynamicFormElementId' AND DATA_TYPE like '%int'") == 1)
			{
				Database.RemovePrimaryKey("CRM", "DynamicFormElement");

				Database.ExecuteNonQuery(
					@"ALTER TABLE [CRM].[DynamicFormElement] ADD [DynamicFormElementIdOld] int NULL
					EXEC('UPDATE [CRM].[DynamicFormElement] SET [DynamicFormElementIdOld] = [DynamicFormElementId]')
					ALTER TABLE [CRM].[DynamicFormElement] DROP COLUMN [DynamicFormElementId]
					ALTER TABLE [CRM].[DynamicFormElement] ADD [DynamicFormElementId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [CRM].[DynamicFormElement] ADD CONSTRAINT [PK_DynamicFormElement] PRIMARY KEY CLUSTERED ([DynamicFormElementId] ASC)
				");
			}

			Database.ExecuteNonQuery(
				@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='LU' AND TABLE_NAME='DynamicFormLocalization' AND COLUMN_NAME='DynamicFormElementId' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[LU].[DynamicFormLocalization].[DynamicFormElementId]', 'DynamicFormElementIdOld', 'COLUMN'
					ALTER TABLE [LU].[DynamicFormLocalization] ADD [DynamicFormElementId] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DynamicFormElementId] = b.[DynamicFormElementId] FROM [LU].[DynamicFormLocalization] a LEFT OUTER JOIN [CRM].[DynamicFormElement] b ON a.[DynamicFormElementIdOld] = b.[DynamicFormElementIdOld]')
					ALTER TABLE [LU].[DynamicFormLocalization] ADD CONSTRAINT [FK_DynamicFormLocalization_DynamicFormElement] FOREIGN KEY([DynamicFormElementId]) REFERENCES [CRM].[DynamicFormElement]([DynamicFormElementId])
				END");

			Database.ExecuteNonQuery(
				@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormResponse' AND COLUMN_NAME='DynamicFormElementKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[CRM].[DynamicFormResponse].[DynamicFormElementKey]', 'DynamicFormElementKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[DynamicFormResponse] ADD [DynamicFormElementKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DynamicFormElementKey] = b.[DynamicFormElementId] FROM [CRM].[DynamicFormResponse] a LEFT OUTER JOIN [CRM].[DynamicFormElement] b ON a.[DynamicFormElementKeyOld] = b.[DynamicFormElementIdOld]')
					DELETE FROM [CRM].[DynamicFormResponse] WHERE [DynamicFormElementKey] IS NULL
					ALTER TABLE [CRM].[DynamicFormResponse] ALTER COLUMN [DynamicFormElementKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[DynamicFormResponse] ALTER COLUMN [DynamicFormElementKeyOld] int NULL
					ALTER TABLE [CRM].[DynamicFormResponse] ADD CONSTRAINT [FK_DynamicFormResponse_DynamicFormElement] FOREIGN KEY([DynamicFormElementKey]) REFERENCES [CRM].[DynamicFormElement]([DynamicFormElementId])
					ALTER TABLE [CRM].[DynamicFormResponse] ADD CONSTRAINT [PK_DynamicFormResponse] PRIMARY KEY CLUSTERED ([DynamicFormReferenceKey] ASC, [DynamicFormElementKey] ASC)
				END");
		}
	}
}
