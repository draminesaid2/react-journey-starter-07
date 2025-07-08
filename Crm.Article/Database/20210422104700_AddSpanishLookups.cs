using System;
using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Database.Modify
{
	[Migration(20210422104700)]
	public class AddSpanishLookups : Migration
	{
		public override void Up()
		{
			string tableName = "[LU].[ArticleRelationshipType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Value", "Name", "InverseName", "ArticleTypes", "HasQuantity", "Language", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Accessory', 'Accesorio', 'Accesorio de', NULL, 0, 'es', 0");
				InsertLookupValue(tableName, columns, "'Set', 'Set', 'Utilizado en el set', 'Set', 1, 'es', 0");
				InsertLookupValue(tableName, columns, "'VariableValue', 'Valor variable', 'Opción de la variable', NULL, 0, 'es', 0");
			}
			tableName = "[LU].[ArticleType]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value" };
				InsertLookupValue(tableName, columns, "'Variable', 'es', 'Variable'");
				InsertLookupValue(tableName, columns, "'Material', 'es', 'Material'");
				InsertLookupValue(tableName, columns, "'Elemento del set', 'es', 'Set'");
				InsertLookupValue(tableName, columns, "'Coste', 'es', 'Cost'");
				InsertLookupValue(tableName, columns, "'Base de configuración', 'es', 'ConfigurationBase'");
				InsertLookupValue(tableName, columns, "'Herramienta', 'es', 'Tool'");
				InsertLookupValue(tableName, columns, "'Servicio', 'es', 'Service'");
			}
			tableName = "[LU].[QuantityUnit]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'Piezas', 'es', 'Stk', 0, 0");
			}
			tableName = "[LU].[ArticleDescription]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Favorite", "SortOrder" };
				InsertLookupValue(tableName, columns, "'EC480EL', 'es', 'EC480EL', 0, 0");
				InsertLookupValue(tableName, columns, "'EC160EL', 'es', 'EC160EL', 0, 0");
				InsertLookupValue(tableName, columns, "'A60H', 'es', 'A60H', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H', 'es', 'L150H', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H', 'es', 'L180H', 0, 0");
				InsertLookupValue(tableName, columns, "'A60H-Motor', 'es', 'A60H-Motor', 0, 0");
				InsertLookupValue(tableName, columns, "'A60H-Elektrik', 'es', 'A60H-Elektrik', 0, 0");
				InsertLookupValue(tableName, columns, "'A60H-Service', 'es', 'A60H-Service', 0, 0");
				InsertLookupValue(tableName, columns, "'A60H-Garantie', 'es', 'A60H-Garantie', 0, 0");
				InsertLookupValue(tableName, columns, "'A60H-Felge', 'es', 'A60H-Felge', 0, 0");
				InsertLookupValue(tableName, columns, "'A60H-Kotflügel', 'es', 'A60H-Kotflügel', 0, 0");
				InsertLookupValue(tableName, columns, "'A60H-Reifen', 'es', 'A60H-Reifen', 0, 0");
				InsertLookupValue(tableName, columns, "'A60H-Öle', 'es', 'A60H-Öle', 0, 0");
				InsertLookupValue(tableName, columns, "'A60H-Fahrerausstattung', 'es', 'A60H-Fahrerausstattung', 0, 0");
				InsertLookupValue(tableName, columns, "'A60H-Multimedia', 'es', 'A60H-Multimedia', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Motor', 'es', 'L150H-Motor', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Elektrik', 'es', 'L150H-Elektrik', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Service', 'es', 'L150H-Service', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Garantie', 'es', 'L150H-Garantie', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Schaufel', 'es', 'L150H-Schaufel', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Felge', 'es', 'L150H-Felge', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Kotflügel', 'es', 'L150H-Kotflügel', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Reifen', 'es', 'L150H-Reifen', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Öle', 'es', 'L150H-Öle', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Fahrerausstattung', 'es', 'L150H-Fahrerausstattung', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Multimedia', 'es', 'L150H-Multimedia', 0, 0");
				InsertLookupValue(tableName, columns, "'L150H-Sicherheitseinrichtungen', 'es', 'L150H-Sicherheitseinrichtungen', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Motor', 'es', 'L180H-Motor', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Elektrik', 'es', 'L180H-Elektrik', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Service', 'es', 'L180H-Service', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Garantie', 'es', 'L180H-Garantie', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Schaufel', 'es', 'L180H-Schaufel', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Felge', 'es', 'L180H-Felge', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Kotflügel', 'es', 'L180H-Kotflügel', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Reifen', 'es', 'L180H-Reifen', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Öle', 'es', 'L180H-Öle', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Fahrerausstattung', 'es', 'L180H-Fahrerausstattung', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Multimedia', 'es', 'L180H-Multimedia', 0, 0");
				InsertLookupValue(tableName, columns, "'L180H-Sicherheitseinrichtungen', 'es', 'L180H-Sicherheitseinrichtungen', 0, 0");
				InsertLookupValue(tableName, columns, "'X942832', 'es', 'X942832', 0, 0");
				InsertLookupValue(tableName, columns, "'X165609', 'es', 'X165609', 0, 0");
				InsertLookupValue(tableName, columns, "'X140264', 'es', 'X140264', 0, 0");
				InsertLookupValue(tableName, columns, "'X145123', 'es', 'X145123', 0, 0");
				InsertLookupValue(tableName, columns, "'X333537', 'es', 'X333537', 0, 0");
				InsertLookupValue(tableName, columns, "'X617124', 'es', 'X617124', 0, 0");
				InsertLookupValue(tableName, columns, "'X609485', 'es', 'X609485', 0, 0");
				InsertLookupValue(tableName, columns, "'X172048', 'es', 'X172048', 0, 0");
				InsertLookupValue(tableName, columns, "'X193387', 'es', 'X193387', 0, 0");
				InsertLookupValue(tableName, columns, "'X118477', 'es', 'X118477', 0, 0");
				InsertLookupValue(tableName, columns, "'X141200', 'es', 'X141200', 0, 0");
				InsertLookupValue(tableName, columns, "'X936409', 'es', 'X936409', 0, 0");
				InsertLookupValue(tableName, columns, "'X308614', 'es', 'X308614', 0, 0");
				InsertLookupValue(tableName, columns, "'X846482', 'es', 'X846482', 0, 0");
				InsertLookupValue(tableName, columns, "'X983142', 'es', 'X983142', 0, 0");
				InsertLookupValue(tableName, columns, "'X426837', 'es', 'X426837', 0, 0");
				InsertLookupValue(tableName, columns, "'X555185', 'es', 'X555185', 0, 0");
				InsertLookupValue(tableName, columns, "'X119179', 'es', 'X119179', 0, 0");
				InsertLookupValue(tableName, columns, "'X777852', 'es', 'X777852', 0, 0");
				InsertLookupValue(tableName, columns, "'X519575', 'es', 'X519575', 0, 0");
				InsertLookupValue(tableName, columns, "'X797561', 'es', 'X797561', 0, 0");
				InsertLookupValue(tableName, columns, "'X997524', 'es', 'X997524', 0, 0");
				InsertLookupValue(tableName, columns, "'X439344', 'es', 'X439344', 0, 0");
				InsertLookupValue(tableName, columns, "'X695952', 'es', 'X695952', 0, 0");
				InsertLookupValue(tableName, columns, "'X315492', 'es', 'X315492', 0, 0");
				InsertLookupValue(tableName, columns, "'X599144', 'es', 'X599144', 0, 0");
				InsertLookupValue(tableName, columns, "'X280359', 'es', 'X280359', 0, 0");
				InsertLookupValue(tableName, columns, "'X712884', 'es', 'X712884', 0, 0");
				InsertLookupValue(tableName, columns, "'X416201', 'es', 'X416201', 0, 0");
				InsertLookupValue(tableName, columns, "'X488742', 'es', 'X488742', 0, 0");
				InsertLookupValue(tableName, columns, "'X212215', 'es', 'X212215', 0, 0");
				InsertLookupValue(tableName, columns, "'X270414', 'es', 'X270414', 0, 0");
				InsertLookupValue(tableName, columns, "'X507781', 'es', 'X507781', 0, 0");
				InsertLookupValue(tableName, columns, "'X260333', 'es', 'X260333', 0, 0");
				InsertLookupValue(tableName, columns, "'Coste C100', 'es', 'Coste C100', 0, 0");
				InsertLookupValue(tableName, columns, "'Coste C200 extra', 'es', 'Coste C200 extra', 0, 0");
			}
			tableName = "[LU].[ArticleGroup]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Remark" };
				InsertLookupValue(tableName, columns, "'Maquinaria de construcción', 'es', 'Construction', NULL");
				InsertLookupValue(tableName, columns, "'Bicicletas', 'es', 'Bike', NULL");
				InsertLookupValue(tableName, columns, "'Base', 'es', 'Basis', NULL");
				InsertLookupValue(tableName, columns, "'Neumáticos', 'es', 'Tires', NULL");
				InsertLookupValue(tableName, columns, "'Hidráulica', 'es', 'Hydraulics', NULL");
				InsertLookupValue(tableName, columns, "'Cabina', 'es', 'Cabin', NULL");
			}
			tableName = "[LU].[ArticleGroup2]";
			if (Database.TableExists(tableName))
			{
				string[] columns = { "Name", "Language", "Value", "Remark" };
				InsertLookupValue(tableName, columns, "'E-Bikes', 'es', 'E-Bike', NULL");
				InsertLookupValue(tableName, columns, "'Bicicletas de carrera', 'es', 'Race', NULL");
				InsertLookupValue(tableName, columns, "'Cabrestante', 'es', 'Hauler', NULL");
				InsertLookupValue(tableName, columns, "'Pala cargadora', 'es', 'WheelLoader', NULL");
				InsertLookupValue(tableName, columns, "'Pala excavadora', 'es', 'Excavator', NULL");
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