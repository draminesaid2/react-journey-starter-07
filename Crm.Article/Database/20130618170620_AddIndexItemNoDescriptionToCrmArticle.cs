namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130618170620)]
	public class AddIndexItemNoDescriptionToCrmArticle : Migration
	{
		public override void Up()
		{
			var sb = new StringBuilder();
			sb.AppendLine("IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_Article_ItemNo_Description')");
			sb.AppendLine("CREATE NONCLUSTERED INDEX [IX_Article_ItemNo_Description] ON [CRM].[Article] ([ItemNo]) INCLUDE ([Description]);");
			Database.ExecuteNonQuery(sb.ToString());
		}
		public override void Down()
		{
		}
	}
}