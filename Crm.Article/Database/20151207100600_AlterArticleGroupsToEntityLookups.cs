namespace Crm.Article.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20151207100600)]
	public class AlterArticleGroupsToEntityLookups : Migration
	{
		public override void Up()
		{
			for (int i = 1; i <= 5; i++)
			{
				var table = "LU.ArticleGroup";
				if (i > 1)
				{
					table += i;
				}
				Database.AddColumnIfNotExisting(table, new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
				Database.AddColumnIfNotExisting(table, new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
				Database.AddColumnIfNotExisting(table, new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
				Database.AddColumnIfNotExisting(table, new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
				Database.AddColumnIfNotExisting(table, new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
			}
		}
	}
}