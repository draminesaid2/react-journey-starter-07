namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20150421105903)]
	public class AddLegacyVersionToCrmDynamicFormElement : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.DynamicFormElement", new Column("LegacyVersion", DbType.Int64, ColumnProperty.Null));
		}
	}
}