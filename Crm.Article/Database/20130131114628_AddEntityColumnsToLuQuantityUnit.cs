namespace Crm.Article.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130131114628)]
	public class AddEntityColumnsToLuQuantityUnit : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("LU.QuantityUnit", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "getUtcDate()"));
			Database.AddColumnIfNotExisting("LU.QuantityUnit", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "getUtcDate()"));
			Database.AddColumnIfNotExisting("LU.QuantityUnit", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("LU.QuantityUnit", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("LU.QuantityUnit", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
		public override void Down()
		{
			Database.RemoveColumnIfExisting("LU.QuantityUnit", "CreateDate");
			Database.RemoveColumnIfExisting("LU.QuantityUnit", "ModifyDate");
			Database.RemoveColumnIfExisting("LU.QuantityUnit", "CreateUser");
			Database.RemoveColumnIfExisting("LU.QuantityUnit", "ModifyUser");
			Database.RemoveColumnIfExisting("LU.QuantityUnit", "IsActive");
		}
	}
}