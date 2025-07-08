namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework; 

	[Migration(20190104132200)]
	public class AddTableCrmDynamicFormElementRuleCondition : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[CRM].[DynamicFormElementRuleCondition]"))
			{
				Database.AddTable(
					"CRM.DynamicFormElementRuleCondition",
					new Column("DynamicFormElementRuleConditionId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("DynamicFormElementId", DbType.Guid, ColumnProperty.NotNull),
					new Column("DynamicFormElementRuleId", DbType.Guid, ColumnProperty.NotNull),
					new Column("Filter", DbType.String, ColumnProperty.NotNull),
					new Column("Value", DbType.String, ColumnProperty.Null),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
				Database.AddForeignKey("FK_DynamicFormElementRuleCondition_DynamicFormElementRule", "[CRM].[DynamicFormElementRuleCondition]", "DynamicFormElementRuleId", "[CRM].[DynamicFormElementRule]", "DynamicFormElementRuleId");
				Database.AddForeignKey("FK_DynamicFormElementRuleCondition_DynamicFormElement", "[CRM].[DynamicFormElementRuleCondition]", "DynamicFormElementId", "[CRM].[DynamicFormElement]", "DynamicFormElementId");
			}
		}
	}
}
