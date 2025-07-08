namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240220111000)]
	public class IncreaseArticleRelationshipQuantityUnitSize : Migration
	{
		public override void Up()
		{
			const string schema = "CRM";
			const string table = "ArticleRelationship";
			const string column = "QuantityUnit";

			if (Database.ColumnExists($"[{schema}].[{table}]", column))
			{
				var length = (int)Database.ExecuteScalar(
					$@"	SELECT CHARACTER_MAXIMUM_LENGTH
						FROM INFORMATION_SCHEMA.COLUMNS
						WHERE TABLE_SCHEMA = '{schema}' AND
							TABLE_NAME = '{table}' AND
							COLUMN_NAME = '{column}'");
				if (length < 20)
				{
					Database.ExecuteNonQuery($"ALTER TABLE [{schema}].[{table}] ALTER COLUMN [{column}] NVARCHAR(20)");
				}
			}
		}
	}
}
