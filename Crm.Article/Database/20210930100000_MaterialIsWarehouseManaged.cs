namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210930100000)]
	public class MaterialIsWarehouseManaged : Migration
	{
		public override void Up()
		{
			Database.AddColumn("CRM.Article", new Column("IsWarehouseManaged")
			{
				Type = System.Data.DbType.Boolean,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = true
			});
		}
	}
}
