namespace Crm.Article.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20151207105100)]
	public class AddParentArticleGroupToArticleGroups : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("LU.ArticleGroup2", new Column("ParentArticleGroup", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("LU.ArticleGroup3", new Column("ParentArticleGroup", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("LU.ArticleGroup4", new Column("ParentArticleGroup", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("LU.ArticleGroup5", new Column("ParentArticleGroup", DbType.String, 50, ColumnProperty.Null));
		}
	}
}