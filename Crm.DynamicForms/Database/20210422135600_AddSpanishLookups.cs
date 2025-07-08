using System;
using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Database.Modify
{
	[Migration(20210422135600)]
	public class AddSpanishLookups : Migration
	{
		public override void Up()
		{
			string tableName = "[LU].[DynamicFormCategory]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value" };
				InsertLookupValue(tableName, columns, "'Ficha técnica', 'es', 'AttributeForm'");
				InsertLookupValue(tableName, columns, "'Checklist', 'es', 'Checklist'");
				InsertLookupValue(tableName, columns, "'Checklist de Caso de Servicio', 'es', 'ServiceCaseChecklist'");
				InsertLookupValue(tableName, columns, "'Informe de visita', 'es', 'VisitReport'");
			}
			tableName = "[LU].[DynamicFormStatus]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Value", "Name", "Language", "Favorite", "SortOrder", "Color" };
				InsertLookupValue(tableName, columns, "'Released', 'Publicado', 'es', 0, 1, '#4CAF50'");
				InsertLookupValue(tableName, columns, "'Draft', 'Borrador', 'es', 1, 0, '#FFC107'");
				InsertLookupValue(tableName, columns, "'Disabled', 'Inactivo', 'es', 0, 2, '#9E9E9E'");
			}
		}
		private void InsertLookupValue(string tableName, string[] columns, string values, bool hasIsActiveColumn = true)
		{
			int keyColumnIndex = Array.IndexOf(columns, "Value");
			string keyValue = values.Split(',')[keyColumnIndex].Trim(new char[] { ' ', '\'' });
			if ((int)Database.ExecuteScalar($"SELECT COUNT(*) FROM {tableName} WHERE {(hasIsActiveColumn ? "[IsActive]" : 1)} = 1 AND [Value] = '{keyValue}'") > 0)
			{
				Database.ExecuteNonQuery($"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES ({values})");
			}
		}
	}
}
