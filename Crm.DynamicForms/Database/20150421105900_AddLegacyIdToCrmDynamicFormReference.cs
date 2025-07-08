namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20150421105900)]
	public class AddLegacyIdToCrmDynamicFormReference : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.DynamicFormReference", new Column("LegacyId", DbType.String, 50, ColumnProperty.Null));
		}
	}
}