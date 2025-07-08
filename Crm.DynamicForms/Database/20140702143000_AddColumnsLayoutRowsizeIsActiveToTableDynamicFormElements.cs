namespace Crm.DynamicForms.Database
{
	using System.Data;

using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140702143000)]
	public class AddColumnsLayoutRowsizeIsActiveToTableDynamicFormElements:Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("CRM.DynamicFormElement", new Column("Title", DbType.String, 255, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("CRM.DynamicFormElement", new Column("Layout", DbType.Int32, ColumnProperty.NotNull, 1));
			Database.AddColumnIfNotExisting("CRM.DynamicFormElement", new Column("Rowsize", DbType.Int32, ColumnProperty.NotNull, 2));
			Database.AddColumnIfNotExisting("CRM.DynamicForm", new Column("IsActive", DbType.Int32, ColumnProperty.NotNull, 1));
		}
	}
}
