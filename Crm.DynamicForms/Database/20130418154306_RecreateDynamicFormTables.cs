namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130418154306)]
	public class RecreateDynamicFormTables : Migration
	{
		public override void Up()
		{
			Database.RemoveTableIfExisting("[CRM].[DynamicFormElementChoice]");
			Database.RemoveTableIfExisting("[CRM].[DynamicFormResponse]");
			Database.RemoveTableIfExisting("[CRM].[DynamicFormResult]");
			Database.RemoveTableIfExisting("[LU].[DynamicFormCategory]");
			Database.RemoveTableIfExisting("[CRM].[DynamicFormDateTimeElement]");
			Database.RemoveTableIfExisting("[CRM].[DynamicFormNumberElement]");
			Database.RemoveTableIfExisting("[CRM].[DynamicFormStringElement]");
			Database.RemoveTableIfExisting("[CRM].[DynamicFormMultipleChoiceElement]");
			Database.RemoveTableIfExisting("[CRM].[DynamicFormElement]");
			Database.RemoveTableIfExisting("[CRM].[DynamicForm]");

			Database.AddTable("[CRM].[DynamicForm]",
				new Column("DynamicFormId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
				new Column("Title", DbType.String, 255, ColumnProperty.NotNull),
				new Column("Description", DbType.String, 255, ColumnProperty.NotNull),
				new Column("CategoryKey", DbType.String, 10, ColumnProperty.NotNull),
				new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
				new Column("CreateUser", DbType.String, 60, ColumnProperty.NotNull),
				new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
				new Column("ModifyUser", DbType.String, 60, ColumnProperty.NotNull)
				);
			Database.AddTable("[CRM].[DynamicFormElement]",
				new Column("DynamicFormElementId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
				new Column("DynamicFormKey", DbType.Int32, ColumnProperty.NotNull),
				new Column("FormElementType", DbType.String, 60, ColumnProperty.NotNull),
				new Column("Title", DbType.String, 255, ColumnProperty.NotNull),
				new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull),
				new Column("Required", DbType.Boolean, ColumnProperty.Null),
				new Column("Hint", DbType.String, ColumnProperty.Null),
				new Column("Min", DbType.Int32, ColumnProperty.Null),
				new Column("Max", DbType.Int32, ColumnProperty.Null),
				new Column("Randomized", DbType.Boolean, ColumnProperty.Null),
				new Column("Choices", DbType.String, ColumnProperty.Null),
				new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
				new Column("CreateUser", DbType.String, 60, ColumnProperty.NotNull),
				new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
				new Column("ModifyUser", DbType.String, 60, ColumnProperty.NotNull)
				);
			Database.AddTable("[CRM].[DynamicFormReference]",
				new Column("DynamicFormReferenceId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
				new Column("DynamicFormReferenceType", DbType.String, 60, ColumnProperty.NotNull),
				new Column("ReferenceKey", DbType.Int32, ColumnProperty.NotNull),
				new Column("DynamicFormKey", DbType.Int32, ColumnProperty.NotNull),
				new Column("Completed", DbType.Boolean, ColumnProperty.NotNull),
				new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
				new Column("CreateUser", DbType.String, 60, ColumnProperty.NotNull),
				new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
				new Column("ModifyUser", DbType.String, 60, ColumnProperty.NotNull),
				new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull)
				);
			Database.AddTable("[CRM].[DynamicFormResponse]",
				new Column("DynamicFormReferenceKey", DbType.Int32, ColumnProperty.NotNull),
				new Column("DynamicFormElementKey", DbType.Int32, ColumnProperty.NotNull),
				new Column("DynamicFormElementType", DbType.String, 60, ColumnProperty.NotNull),
				new Column("Value", DbType.String, ColumnProperty.Null)
				);
			Database.AddTable("[LU].[DynamicFormCategory]",
				new Column("DynamicFormCategoryId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
				new Column("Name", DbType.String, 60, ColumnProperty.NotNull),
				new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
				new Column("Value", DbType.String, 60, ColumnProperty.NotNull),
				new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
				new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0)
				);

			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DynamicFormElement] ALTER COLUMN Hint NVARCHAR(MAX)");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DynamicFormElement] ALTER COLUMN Choices NVARCHAR(MAX)");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DynamicFormResponse] ALTER COLUMN Value NVARCHAR(MAX)");
			Database.AddPrimaryKey("PK_DynamicFormResponse", "[CRM].[DynamicFormResponse]", "DynamicFormReferenceKey", "DynamicFormElementKey");
			Database.AddForeignKey("FK_DynamicFormElement_DynamicForm", "[CRM].[DynamicFormElement]", "DynamicFormKey", "[CRM].[DynamicForm]", "DynamicFormId");
			Database.AddForeignKey("FK_DynamicFormReference_DynamicForm", "[CRM].[DynamicFormReference]", "DynamicFormKey", "[CRM].[DynamicForm]", "DynamicFormId");
			Database.AddForeignKey("FK_DynamicFormResponse_DynamicFormReference", "[CRM].[DynamicFormResponse]", "DynamicFormReferenceKey", "[CRM].[DynamicFormReference]", "DynamicFormReferenceId");
			Database.AddForeignKey("FK_DynamicFormResponse_DynamicFormElement", "[CRM].[DynamicFormResponse]", "DynamicFormElementKey", "[CRM].[DynamicFormElement]", "DynamicFormElementId");
		}

		public override void Down()
		{
		}
	}
}