namespace Crm.Article.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140115150706)]
	public class AddColumnSerialRequiredToCrmArticle : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.Article", new Column("SerialRequired", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}