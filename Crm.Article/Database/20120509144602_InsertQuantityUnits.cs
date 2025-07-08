namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120509144602)]
	public class InsertQuantityUnits : Migration
	{
		public override void Up()
		{
			StringBuilder sb = new StringBuilder();

			var doesSmsQuantityUnitExist = Database.TableExists("[SMS].[QuantityUnit]");
			var oldQuantityUnitCount = doesSmsQuantityUnitExist
																		? (int)Database.ExecuteScalar("SELECT COUNT(*) FROM [SMS].[QuantityUnit]")
																		: 0;

			if (Database.TableExists("[SMS].[QuantityUnit]"))
			{
				sb.AppendLine("INSERT INTO [LU].[QuantityUnit] (Name, Language, Value)");
				sb.AppendLine("SELECT Name, Language, Value");
				sb.AppendLine("FROM [SMS].[QuantityUnit]");
			}
			else
			{
				sb.AppendLine("INSERT INTO [LU].[QuantityUnit] (Name, Language, Value)");
				sb.AppendLine("VALUES");
				sb.AppendLine("('Stück',		'de',	'Pieces'),");
				sb.AppendLine("('Pieces',		'en',	'Pieces')");
			}
			Database.ExecuteNonQuery(sb.ToString());

			if (doesSmsQuantityUnitExist)
				Database.ExecuteNonQuery("sp_rename '[SMS].[QuantityUnit]', 'Old_QuantityUnit'");
		}
		public override void Down()
		{
			
		}
	}
}
