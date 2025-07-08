using System.Data;
using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Article.Database
{
	[Migration(20141021000099)]
	public class AddQuantityStep : Migration
	{
		public override void Up()
		{
			Database.AddColumn("[CRM].[Article]", new Column("QuantityStep", DbType.Decimal, ColumnProperty.NotNull, 1));
		}
		public override void Down()
		{
		}
	}
}