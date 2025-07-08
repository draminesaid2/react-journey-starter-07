namespace Crm.DynamicForms.Database
{
	using System;
	using System.Data;
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140626133900)]
	public class AlterColumnDescriptionMakeLengthMax : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("CRM.DynamicForm", new Column("Description", DbType.String, Int32.MaxValue, ColumnProperty.Null));
		}
		public override void Down()
		{
		}
	}
}