namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160623165800)]
	public class ChangeCrmDynamicFormReferenceIdToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_ServiceOrderChecklist_DynamicFormReference')
					BEGIN
						ALTER TABLE[SMS].[ServiceOrderChecklist] DROP CONSTRAINT[FK_ServiceOrderChecklist_DynamicFormReference]
					END");

			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'FK_DynamicFormResponse_DynamicFormReference')
					BEGIN
						ALTER TABLE [CRM].[DynamicFormResponse] DROP CONSTRAINT[FK_DynamicFormResponse_DynamicFormReference]
					END");

			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_DynamicFormResponse')
					BEGIN
						ALTER TABLE [CRM].[DynamicFormResponse] DROP CONSTRAINT[PK_DynamicFormResponse]
					END");
			
			Database.ExecuteNonQuery(@" IF EXISTS (SELECT * FROM sys.objects WHERE NAME = 'PK_DynamicFormReference')
					BEGIN
						ALTER TABLE [CRM].[DynamicFormReference] DROP CONSTRAINT[PK_DynamicFormReference]
					END");
			
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormReference' AND COLUMN_NAME='DynamicFormReferenceId' AND DATA_TYPE like '%int')
				BEGIN
				ALTER TABLE [CRM].[DynamicFormReference] ADD [DynamicFormReferenceIdOld] bigint NULL
					EXEC('UPDATE [CRM].[DynamicFormReference] SET [DynamicFormReferenceIdOld] = [DynamicFormReferenceId]')
					ALTER TABLE [CRM].[DynamicFormReference] DROP COLUMN [DynamicFormReferenceId]
					ALTER TABLE [CRM].[DynamicFormReference] ADD [DynamicFormReferenceId] uniqueidentifier NOT NULL DEFAULT(NEWSEQUENTIALID())
					ALTER TABLE [CRM].[DynamicFormReference] ADD  CONSTRAINT [PK_DynamicFormReference] PRIMARY KEY CLUSTERED ([DynamicFormReferenceId] ASC)
				END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormResponse' AND COLUMN_NAME='DynamicFormReferenceKey' AND DATA_TYPE like '%int')
				BEGIN
					EXEC sp_rename '[CRM].[DynamicFormResponse].[DynamicFormReferenceKey]', 'DynamicFormReferenceKeyOld', 'COLUMN'
					ALTER TABLE [CRM].[DynamicFormResponse] ADD [DynamicFormReferenceKey] uniqueidentifier NULL
					EXEC('UPDATE a SET a.[DynamicFormReferenceKey] = b.[DynamicFormReferenceId] FROM [CRM].[DynamicFormResponse] a LEFT OUTER JOIN [CRM].[DynamicFormReference] b ON a.[DynamicFormReferenceKeyOld] = b.[DynamicFormReferenceIdOld]')
					ALTER TABLE [CRM].[DynamicFormResponse] ALTER COLUMN [DynamicFormReferenceKey] uniqueidentifier NOT NULL
					ALTER TABLE [CRM].[DynamicFormResponse] ALTER COLUMN [DynamicFormReferenceKeyOld] bigint NULL
					ALTER TABLE [CRM].[DynamicFormResponse] ADD CONSTRAINT [PK_DynamicFormResponse] PRIMARY KEY CLUSTERED ([DynamicFormReferenceKey] ASC, [DynamicFormElementKey] ASC)
					ALTER TABLE [CRM].[DynamicFormResponse] ADD CONSTRAINT [FK_DynamicFormResponse_DynamicFormReference] FOREIGN KEY([DynamicFormReferenceKey]) REFERENCES [CRM].[DynamicFormReference]([DynamicFormReferenceId])
				END");
		}
	}
}