namespace Crm.DynamicForms.Database
{
	using System;
	using System.Collections.Generic;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
	using Crm.Library.Data.NHibernateProvider.UserTypes;

	[Migration(20170609112400)]
	public class AddTableLuDynamicFormLocalization : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[DynamicFormLocalization]"))
			{
				var defaultLanguageTable = Database.TableExists("[CRM].[Site]") ? "[CRM].[Site]" : "[dbo].[Domain]";
				var defaultLanguageColumn = Database.TableExists("[CRM].[Site]") ? "DefaultLanguage" : "DefaultLanguageKey";

				Database.AddTable("LU.DynamicFormLocalization",
					new Column("DynamicFormLocalizationId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("DynamicFormId", DbType.Int32, ColumnProperty.NotNull),
					new Column("DynamicFormElementId", DbType.Int32, ColumnProperty.Null),
					new Column("ChoiceIndex", DbType.Int32, ColumnProperty.Null),
					new Column("Value", DbType.String, ColumnProperty.Null),
					new Column("Name", DbType.String, Int32.MaxValue, ColumnProperty.NotNull),
					new Column("Hint", DbType.String, Int32.MaxValue, ColumnProperty.Null),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("TenantKey", DbType.Int32, ColumnProperty.Null)
				);
				Database.AddForeignKey("FK_DynamicFormLocalization_DynamicForm", "[LU].[DynamicFormLocalization]", "DynamicFormId", "[CRM].[DynamicForm]", "DynamicFormId");
				Database.AddForeignKey("FK_DynamicFormLocalization_DynamicFormElement", "[LU].[DynamicFormLocalization]", "DynamicFormElementId", "[CRM].[DynamicFormElement]", "DynamicFormElementId");

				Database.ExecuteNonQuery($@"INSERT INTO [LU].[DynamicFormLocalization] 
					([DynamicFormId], [Name], [Hint], [Language])
					SELECT  [DynamicFormId],
							COALESCE([Title], ''),
							[Description],
							SUBSTRING([{defaultLanguageColumn}], 0, 3)
					FROM [CRM].[DynamicForm] CROSS APPLY {defaultLanguageTable}");

				Database.ExecuteNonQuery($@"INSERT INTO [LU].[DynamicFormLocalization] 
					([DynamicFormId], [DynamicFormElementId], [Name], [Hint], [Language])
					SELECT  [DynamicFormKey],
							[DynamicFormElementId],
							COALESCE([Title], ''),
							[Hint],
							SUBSTRING([{defaultLanguageColumn}], 0, 3)
					FROM [CRM].[DynamicFormElement] CROSS APPLY {defaultLanguageTable}");

				var result = Database.ExecuteQuery($@"SELECT [DynamicFormKey], [DynamicFormElementId], COALESCE([Choices], ''), SUBSTRING([{defaultLanguageColumn}], 0, 3)
					FROM [CRM].[DynamicFormElement] CROSS APPLY {defaultLanguageTable}
					WHERE [Choices] IS NOT NULL OR [FormElementType] IN ('CheckBoxList', 'DropDown', 'RadioButtonList')");
				var queries = new List<string>();
				while (result.Read())
				{
					var row = (IDataRecord)result;
					var dynamicFormId = row.GetInt32(0);
					var dynamicFormElementId = row.GetInt32(1);
					var choices = new DelimitedString(row.GetString(2));
					var language = row.GetString(3);
					for (int i = 0; i < choices.Count; i++)
					{
						queries.Add($@"INSERT INTO [LU].[DynamicFormLocalization] 
							([DynamicFormId], [DynamicFormElementId], [ChoiceIndex], [Name], [Language])
							VALUES ({dynamicFormId}, {dynamicFormElementId}, {i}, '{choices[i]?.Replace("'", "''") ?? String.Empty}', '{language}')");
					}
					queries.Add($"UPDATE [CRM].[DynamicFormElement] SET [Choices] = '{Math.Max(choices.Count, 1)}' WHERE [DynamicFormElementId] = {dynamicFormElementId}");
				}
				result.Close();
				foreach (var query in queries)
				{
					Database.ExecuteNonQuery(query);
				}
			}
			Database.RemoveColumnIfExisting("[CRM].[DynamicForm]", "Title");
			Database.RemoveColumnIfExisting("[CRM].[DynamicForm]", "Description");
			Database.RemoveColumnIfExisting("[CRM].[DynamicFormElement]", "Title");
			Database.RemoveColumnIfExisting("[CRM].[DynamicFormElement]", "Hint");
			Database.ChangeColumn("[CRM].[DynamicFormElement]", new Column("Choices", DbType.Int32, ColumnProperty.Null));
		}
	}
}