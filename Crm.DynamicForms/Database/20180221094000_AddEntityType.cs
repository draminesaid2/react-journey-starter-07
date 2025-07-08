namespace Crm.DynamicForms.Database
{
	using Crm.DynamicForms.Model;
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	[Migration(20180221094000)]
	public class AddEntityType : Migration
	{
		public override void Up()
		{
			var helper = new UnicoreMigrationHelper(Database);
			helper.AddEntityTypeAndAuthDataColumnIfNeeded<DynamicFormLanguage>("CRM", "DynamicFormLanguage");
		}
	}
}