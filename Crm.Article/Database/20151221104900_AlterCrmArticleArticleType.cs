namespace Crm.Article.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20151221104900)]
	public class AlterCrmArticleArticleType : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("CRM.Article", new Column("ArticleType", DbType.String, 20, ColumnProperty.NotNull));
		}
	}
}