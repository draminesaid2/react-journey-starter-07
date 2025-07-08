namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20150421105902)]
	public class AddLegacyIdToCrmDynamicFormElement : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.DynamicFormElement", new Column("LegacyId", DbType.String, 50, ColumnProperty.Null));
		}
	}
}