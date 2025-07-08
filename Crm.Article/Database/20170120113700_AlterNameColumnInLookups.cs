using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.Service.Database
{
	[Migration(20170120113700)]
	public class AlterNameColumnInLookups : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [LU].[QuantityUnit] ALTER COLUMN [Name] NVARCHAR(50) NOT NULL");
		}
	}
}