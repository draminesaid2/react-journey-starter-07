namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20121114121544)]
    public class AlterIntColumnToBitForBoolProperties : Migration
    {
        public override void Up()
        {
            StringBuilder query = new StringBuilder();

            query.AppendLine("ALTER TABLE CRM.Article ALTER COLUMN IsSerial bit not null");
            query.AppendLine("ALTER TABLE CRM.Article ALTER COLUMN IsBatch bit not null");
            query.AppendLine("ALTER TABLE CRM.Article ALTER COLUMN UserFlag01 bit null");
            query.AppendLine("ALTER TABLE CRM.Article ALTER COLUMN UserFlag02 bit null");
            query.AppendLine("ALTER TABLE CRM.Article ALTER COLUMN UserFlag03 bit null");
            query.AppendLine("ALTER TABLE CRM.Article ALTER COLUMN UserFlag04 bit null");
            query.AppendLine("ALTER TABLE CRM.Article ALTER COLUMN UserFlag05 bit null");
            query.AppendLine("ALTER TABLE CRM.Article ALTER COLUMN DangerousGoodsFlag bit not null");

            Database.ExecuteNonQuery(query.ToString());
        }

        public override void Down()
        {
        }
    }
}
