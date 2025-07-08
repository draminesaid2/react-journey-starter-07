namespace Crm.AttributeForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20191030135000)]
	public class AddColumnsToCrmDynamicFormElement : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.DynamicFormElement", new Column("DisplayOnItemTemplate", DbType.Boolean, 1, ColumnProperty.NotNull, false));
			Database.AddColumnIfNotExisting("CRM.DynamicFormElement", new Column("Filterable", DbType.Boolean, 1, ColumnProperty.NotNull, false));
		}
	}
}