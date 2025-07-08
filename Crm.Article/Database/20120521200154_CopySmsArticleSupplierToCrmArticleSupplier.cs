namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120521200154)]
	public class CopySmsArticleSupplierToCrmArticleSupplier : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[ArticleSupplier]"))
			{
				StringBuilder sb = new StringBuilder();

				sb.AppendLine("INSERT INTO CRM.ArticleSupplier (");
				sb.AppendLine("	[ItemNo],");
				sb.AppendLine("	[SupplierContactId],");
				sb.AppendLine("	[ExternalItemNo],");
				sb.AppendLine("	[ExternalDescription],");
				sb.AppendLine("	[CreateDate],");
				sb.AppendLine("	[CreatorId],");
				sb.AppendLine("	[ModifyDate],");
				sb.AppendLine("	[ModifyId],");
				sb.AppendLine("	[Favorite],");
				sb.AppendLine("	[SortOrder])");
				sb.AppendLine("SELECT ");
				sb.AppendLine("	[ItemNo],");
				sb.AppendLine("	[SupplierContactId],");
				sb.AppendLine("	[ExternalItemNo],");
				sb.AppendLine("	[ExternalDescription],");
				sb.AppendLine("	[CreateDate],");
				sb.AppendLine("	[CreatorId],");
				sb.AppendLine("	[ModifyDate],");
				sb.AppendLine("	[ModifyId],");
				sb.AppendLine("	[Favorite],");
				sb.AppendLine("	[SortOrder]");
				sb.AppendLine("FROM SMS.ArticleSupplier");

				Database.ExecuteNonQuery(sb.ToString());

				Database.ExecuteNonQuery("sp_rename '[SMS].[ArticleSupplier]', 'Old_ArticleSupplier'");
			}
		}
		public override void Down()
		{
			
		}
	}
}
