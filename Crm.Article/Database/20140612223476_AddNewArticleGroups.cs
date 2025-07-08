using System.Data;
using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Article.Database
{
	[Migration(20140612223476)]
	public class AddNewArticleGroups : Migration
	{
		public override void Up()
		{
			const int startFrom = 2;
			const int createUntil = 5;
			const string articleTable = "[CRM].[Article]";
			for (var i = startFrom; i <= createUntil; i++)
			{
				var tableName = "[LU].[ArticleGroup" + i + "]";
				if (!Database.TableExists(tableName))
				{
					Database.AddTable(tableName,
						new Column("ArticleGroupId", DbType.Int16, ColumnProperty.Identity),
						new Column("Name", DbType.String, 150),
						new Column("Language", DbType.String, 2),
						new Column("Value", DbType.String, 50),
						new Column("Favorite", DbType.Byte, 1),
						new Column("SortOrder", DbType.Byte, 1),
						new Column("Remark", DbType.String, 250),
						new Column("TenantKey", DbType.Int16, ColumnProperty.Null));
				}
				var colName = "ArticleGroup0" + i;
				if (!Database.ColumnExists(articleTable, colName))
				{
					Database.AddColumn(articleTable, new Column(colName, DbType.String, 20));
				}
			}
		}
		public override void Down()
		{
		}
	}
}