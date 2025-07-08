namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200518210000)]
	public class VatLevelTypes : Migration
	{
		public override void Up()
		{
			const string schema = "LU";
			const string table = "VATLevel";
			const string column = "Name";

			var length = (int)Database.ExecuteScalar(
				$@"	SELECT CHARACTER_MAXIMUM_LENGTH
						FROM INFORMATION_SCHEMA.COLUMNS
						WHERE TABLE_SCHEMA = '{schema}' AND
							TABLE_NAME = '{table}' AND
							COLUMN_NAME = '{column}'");
			if (length < 256)
			{
				Database.ExecuteNonQuery($"ALTER TABLE [{schema}].[{table}] ALTER COLUMN [{column}] NVARCHAR(256) NOT NULL");
			}
			Database.ExecuteNonQuery(@"
				DECLARE @defaultConstraint NVARCHAR(MAX)
				SET @defaultConstraint = (
					SELECT NAME
					FROM sys.objects
					WHERE type = 'D' AND object_id = (
						SELECT default_object_id
						FROM sys.columns
						WHERE NAME = 'PercentageValue' AND object_id = (SELECT OBJECT_ID('LU.VATLevel'))
					)
				)
				DECLARE @sql NVARCHAR(MAX)
				SET @sql = 'ALTER TABLE LU.VATLevel DROP CONSTRAINT ' + @defaultConstraint
				EXEC sp_executesql @sql");
			Database.ExecuteNonQuery($"ALTER TABLE [{schema}].[{table}] ALTER COLUMN [PercentageValue] DECIMAL(10,5) NOT NULL");
			Database.ExecuteNonQuery($"ALTER TABLE [{schema}].[{table}] ADD CONSTRAINT DF_PercentageValue DEFAULT 0 FOR PercentageValue");
		}
	}
}