namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140730115200)]
	public class AddColumnCssExtraInDynamicFormElement : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.DynamicFormElement", new Column("CssExtra", DbType.String, 750, ColumnProperty.Null));
		}
	}
}