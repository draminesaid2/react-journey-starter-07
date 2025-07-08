namespace Crm.AttributeForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20191004143000)]
	public class CreateContactTypeColumnInCrmDynamicForm : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.DynamicForm", new Column("ContactType", DbType.String, 50, ColumnProperty.Null));
		}
	}
}