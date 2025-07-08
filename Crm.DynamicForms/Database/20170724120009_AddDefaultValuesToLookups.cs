namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
	using Crm.Library.Extensions;

	[Migration(20170724120009)]
	public class AddDefaultValuesToLookups : Migration
	{
		public override void Up()
		{
			new[]
			{
				new { Schema = "LU", Table = "DynamicFormCategory" }
			}
			.ForEach(x => Database.AddEntityBaseDefaultContraints(x.Schema, x.Table));
		}
	}
}