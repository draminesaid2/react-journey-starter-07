namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160412173000)]
	public class ChangeCrmDynamicFormReferenceToGuid : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='CRM' AND TABLE_NAME='DynamicFormReference' AND COLUMN_NAME='ReferenceKey' AND DATA_TYPE = 'int')
			BEGIN
				EXEC sp_rename '[CRM].[DynamicFormReference].[ReferenceKey]', 'ReferenceKeyOld', 'COLUMN'
				ALTER TABLE [CRM].[DynamicFormReference] ADD [ReferenceKey] uniqueidentifier NULL
				EXEC('UPDATE a SET a.[ReferenceKey] = b.[ContactId] FROM [CRM].[DynamicFormReference] a LEFT OUTER JOIN [CRM].[Contact] b ON a.[ReferenceKeyOld] = b.[ContactIdOld]')
				ALTER TABLE [CRM].[DynamicFormReference] ALTER COLUMN [ReferenceKeyOld] int NULL
				ALTER TABLE [CRM].[DynamicFormReference] ADD CONSTRAINT [FK_DynamicFormReference_Contact] FOREIGN KEY ([ReferenceKey]) REFERENCES [CRM].[Contact] ([ContactId])
			END");
		}
	}
}