namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200514150000)]
	public class QuantityUnitKeyLength : Migration
	{
		public override void Up()
		{
			const string schema = "LU";
			const string table = "QuantityUnit";
			const string column = "Value";

			var length = (int)Database.ExecuteScalar(
				$@"	SELECT CHARACTER_MAXIMUM_LENGTH
						FROM INFORMATION_SCHEMA.COLUMNS
						WHERE TABLE_SCHEMA = '{schema}' AND
							TABLE_NAME = '{table}' AND
							COLUMN_NAME = '{column}'");
			if (length < 20)
			{
				Database.ExecuteNonQuery($"ALTER TABLE [{schema}].[{table}] ALTER COLUMN [{column}] NVARCHAR(20) NOT NULL");
			}
		}
	}
}