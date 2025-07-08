namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130529113124)]
	public class AlterTableDynamicFormMakeDescriptionNullable : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("CRM.DynamicForm", new Column("Description", DbType.String, 255, ColumnProperty.Null));
		}

		public override void Down()
		{
		}
	}
}