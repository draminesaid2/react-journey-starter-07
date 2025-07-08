namespace Crm.AttributeForms.Database
{
	using Crm.AttributeForms.Model;
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	[Migration(20200102132000)]
	public class AddEntityTypeAttributeForm : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<AttributeForm>("CRM", "DynamicFormReference");
		}
	}
}