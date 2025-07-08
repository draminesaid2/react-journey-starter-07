using System.Data;
using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Article.Database
{
	[Migration(20140311222399)]
	public class AddVATLevelLookup : Migration
	{
		public override void Up()
		{
			const string tableName = "[LU].[VATLevel]";
			if (Database.TableExists(tableName))
			{
				return;
			}
			Database.AddTable(tableName,
				new Column("VATLevelId", DbType.Int16, ColumnProperty.Identity),
				new Column("Name", DbType.String, 20, ColumnProperty.NotNull),
				new Column("Language", DbType.StringFixedLength, 2, ColumnProperty.NotNull),
				new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
				new Column("SortOrder", DbType.Int16, ColumnProperty.Null),
				new Column("Value", DbType.String, 30, ColumnProperty.NotNull),
				new Column("PercentageValue", DbType.Int16, ColumnProperty.NotNull, 18),
				new Column("TenantKey", DbType.Int16, ColumnProperty.Null));
		}
		public override void Down()
		{

		}
	}
}