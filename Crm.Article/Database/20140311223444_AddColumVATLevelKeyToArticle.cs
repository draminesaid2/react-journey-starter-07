using System.Data;
using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Article.Database
{
	[Migration(20140311223444)]
	public class AddColumVATLevelKeyToCrmOrderItem : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[CRM].[Article]", "VATLevelKey"))
			{
				Database.AddColumn("[CRM].[Article]", new Column("VATLevelKey", DbType.String, 10, ColumnProperty.Null));
			}
		}
		public override void Down()
		{
		}
	}
}