namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170608160601)]
	public class AddTableLuDynamicFormStatus : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[DynamicFormStatus]"))
			{
				Database.AddTable("LU.DynamicFormStatus",
					new Column("DynamicFormStatusId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("Color", DbType.String, 20, ColumnProperty.NotNull, "'#9E9E9E'"),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("TenantKey", DbType.Int32, ColumnProperty.Null)
				);
				Database.ExecuteNonQuery("INSERT INTO LU.DynamicFormStatus (Value, Name, Language, Favorite, SortOrder, Color) VALUES ('Draft', 'Entwurf', 'de', 1, 0, '#FFC107')");
				Database.ExecuteNonQuery("INSERT INTO LU.DynamicFormStatus (Value, Name, Language, Favorite, SortOrder, Color) VALUES ('Draft', 'Draft', 'en', 1, 0, '#FFC107')");
				Database.ExecuteNonQuery("INSERT INTO LU.DynamicFormStatus (Value, Name, Language, Favorite, SortOrder, Color) VALUES ('Released', 'Freigegeben', 'de', 0, 1, '#4CAF50')");
				Database.ExecuteNonQuery("INSERT INTO LU.DynamicFormStatus (Value, Name, Language, Favorite, SortOrder, Color) VALUES ('Released', 'Released', 'en', 0, 1, '#4CAF50')");
				Database.ExecuteNonQuery("INSERT INTO LU.DynamicFormStatus (Value, Name, Language, Favorite, SortOrder, Color) VALUES ('Disabled', 'Deaktiviert', 'de', 0, 2, '#9E9E9E')");
				Database.ExecuteNonQuery("INSERT INTO LU.DynamicFormStatus (Value, Name, Language, Favorite, SortOrder, Color) VALUES ('Disabled', 'Disabled', 'en', 0, 2, '#9E9E9E')");
			}
		}
	}
}