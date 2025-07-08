namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170608160600)]
	public class AddTableCrmDynamicFormLanguage : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[CRM].[DynamicFormLanguage]"))
			{
				Database.AddTable("CRM.DynamicFormLanguage",
					new Column("DynamicFormLanguageId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("DynamicFormKey", DbType.Int32, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 20, ColumnProperty.NotNull),
					new Column("Status", DbType.String, ColumnProperty.NotNull),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("TenantKey", DbType.Int32, ColumnProperty.Null)
					);
				Database.AddForeignKey("FK_DynamicFormLanguage_DynamicForm", "[CRM].[DynamicFormLanguage]", "DynamicFormKey", "[CRM].[DynamicForm]", "DynamicFormId");
				if (Database.TableExists("[CRM].[Site]"))
				{
					Database.ExecuteNonQuery(@"
					INSERT INTO [CRM].[DynamicFormLanguage] (
						[DynamicFormKey],
						[Language],
						[Status]
					) SELECT
						[DynamicFormId],
						SUBSTRING([DefaultLanguage], 0, 3),
						'Released'
					FROM [CRM].[DynamicForm] CROSS APPLY [CRM].[Site]");
				}
				else
				{
					Database.ExecuteNonQuery(@"
					INSERT INTO [CRM].[DynamicFormLanguage] (
						[DynamicFormKey],
						[Language],
						[Status]
					) SELECT
						[DynamicFormId],
						SUBSTRING([DefaultLanguageKey], 0, 3),
						'Released'
					FROM [CRM].[DynamicForm] CROSS APPLY [dbo].[Domain]");
				}
			}
		}
	}
}
