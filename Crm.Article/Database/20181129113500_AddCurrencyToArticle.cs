namespace Crm.Article.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20181129113500)]
	public class AddCurrencyToArticle : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.Article", new Column("CurrencyKey", DbType.String, 20, ColumnProperty.Null));
		}
	}
}
