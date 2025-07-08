namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20190221190000)]
	public class RemoveObsoleteFields : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfEmpty("CRM.Article", "UserFlag01", 0);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserFlag02", 0);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserFlag03", 0);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserFlag04", 0);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserFlag05", 0);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserString01", null);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserString02", null);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserString03", null);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserString04", null);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserString05", null);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserString05", null);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserDate01", null);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserDate02", null);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserDate03", null);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserReal01", null);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserReal02", null);
			Database.RemoveColumnIfEmpty("CRM.Article", "UserReal03", null);
		}
	}
}