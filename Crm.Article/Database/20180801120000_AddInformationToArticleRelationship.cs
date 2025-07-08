namespace Crm.Article.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180801120000)]
	public class AddInformationToArticleRelationship : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.ArticleRelationship", new Column("Information", DbType.String, ColumnProperty.Null));
		}
	}
}