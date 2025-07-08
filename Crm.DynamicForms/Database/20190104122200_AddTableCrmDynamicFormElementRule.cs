namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190104122200)]
	public class AddTableCrmDynamicFormElementRule : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[CRM].[DynamicFormElementRule]"))
			{
				Database.AddTable(
					"CRM.DynamicFormElementRule",
					new Column("DynamicFormElementRuleId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("DynamicFormId", DbType.Guid, ColumnProperty.NotNull),
					new Column("DynamicFormElementId", DbType.Guid, ColumnProperty.NotNull),
					new Column("MatchType", DbType.String, ColumnProperty.NotNull),
					new Column("Type", DbType.String, ColumnProperty.NotNull),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
				Database.AddForeignKey("FK_DynamicFormElementRule_DynamicForm", "[CRM].[DynamicFormElementRule]", "DynamicFormId", "[CRM].[DynamicForm]", "DynamicFormId");
				Database.AddForeignKey("FK_DynamicFormElementRule_DynamicFormElement", "[CRM].[DynamicFormElementRule]", "DynamicFormElementId", "[CRM].[DynamicFormElement]", "DynamicFormElementId");
			}
		}
	}
}
