namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20200608124500)]
	public class ArticleLookupReferenceLengths : Migration
	{
		public override void Up()
		{
			const string schema = "CRM";
			const string table = "Article";
			if (Database.GetColumnLength(schema, table, "VATLevelKey") < 30)
			{
				Database.ExecuteNonQuery($"ALTER TABLE [{schema}].[{table}] ALTER COLUMN [VATLevelKey] NVARCHAR(30) NULL");
			}
			if (Database.GetColumnLength(schema, table, "QuantityUnit") < 20)
			{
				Database.ExecuteNonQuery($"ALTER TABLE [{schema}].[{table}] ALTER COLUMN [QuantityUnit] NVARCHAR(20) NULL");
			}
		}
	}
}