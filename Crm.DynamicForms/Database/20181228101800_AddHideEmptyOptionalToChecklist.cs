namespace Sms.Checklists.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20190120165400)]
	public class AddHideEmptyOptionalToDynamicForm : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.DynamicForm", new Column("HideEmptyOptional", DbType.Boolean, ColumnProperty.NotNull, false));
		}
	}
}
