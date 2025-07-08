namespace Crm.Article.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20200824094000)]
	public class AddProductFamilyKeyToArticle : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.Article", new Column("ProductFamilyKey", DbType.Guid, ColumnProperty.Null));
			Database.ExecuteNonQuery("ALTER TABLE CRM.Article ADD FOREIGN KEY (ProductFamilyKey) REFERENCES CRM.Contact(ContactId)");
		}
	}
}