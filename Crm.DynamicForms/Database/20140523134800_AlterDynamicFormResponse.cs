namespace Crm.DynamicForms
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140523134800)]
	public class AlterDynamicFormResponse : Migration
	{
		public override void Up()
		{
			// copy old data
			Database.ExecuteNonQuery("select * into [CRM].[DynamicFormResponseCopy] FROM [CRM].[DynamicFormResponse]");

			// truncate table 
			Database.ExecuteNonQuery("truncate table [CRM].[DynamicFormResponse]");

			// migrate
			Database.AddColumnIfNotExisting("[CRM].[DynamicFormResponse]", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull));
			Database.AddColumnIfNotExisting("[CRM].[DynamicFormResponse]", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull));
			Database.AddColumnIfNotExisting("[CRM].[DynamicFormResponse]", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull));
			Database.AddColumnIfNotExisting("[CRM].[DynamicFormResponse]", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull));
			Database.AddColumnIfNotExisting("[CRM].[DynamicFormResponse]", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull));

			// move data back 
			Database.ExecuteNonQuery("insert into [CRM].[DynamicFormResponse] (DynamicFormReferenceKey,DynamicFormElementKey,DynamicFormElementType,Value,TenantKey,CreateUser,  ModifyUser, CreateDate, ModifyDate, IsActive) SELECT DynamicFormReferenceKey, DynamicFormElementKey, DynamicFormElementType, Value, TenantKey, 'migration 20140523134800' as CreateUser, 'migration 20140523134800' as ModifyUser, getutcdate() as CreateDate, getutcdate() as ModifyDate, 1 as IsActive FROM [CRM].[DynamicFormResponseCopy]");

			// delete temp table 
			Database.ExecuteNonQuery("DROP TABLE [CRM].[DynamicFormResponseCopy]");

			// good luck!
		}

		public override void Down()
		{

		}
	}
}