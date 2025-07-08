namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120101020000)]
	public class CrmArticleSupplier : Migration
	{
		public override void Up()
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine("IF NOT EXISTS(select * from sys.tables");
			stringBuilder.AppendLine("where name = 'ArticleSupplier'");
			stringBuilder.AppendLine("and schema_id = schema_id('CRM'))");
			stringBuilder.AppendLine("BEGIN");
			stringBuilder.AppendLine("CREATE TABLE [CRM].[ArticleSupplier](");
			stringBuilder.AppendLine("[ArticleSupplierId] [int] IDENTITY(1,1) NOT NULL,");
			stringBuilder.AppendLine("[ItemNo] [nvarchar](50) NOT NULL,");
			stringBuilder.AppendLine("[SupplierContactId] [int] NOT NULL,");
			stringBuilder.AppendLine("[ExternalItemNo] [nvarchar](30) NULL,");
			stringBuilder.AppendLine("[ExternalDescription] [nvarchar](100) NULL,");
			stringBuilder.AppendLine("[CreateDate] [datetime] NOT NULL,");
			stringBuilder.AppendLine("[CreatorId] [uniqueidentifier] NOT NULL,");
			stringBuilder.AppendLine("[ModifyDate] [datetime] NULL,");
			stringBuilder.AppendLine("[ModifyId] [uniqueidentifier] NULL,");
			stringBuilder.AppendLine("[Favorite] [bit] NOT NULL,");
			stringBuilder.AppendLine("[SortOrder] [int] NOT NULL,");
			stringBuilder.AppendLine("CONSTRAINT [PK_ArticleSupplier] PRIMARY KEY CLUSTERED ");
			stringBuilder.AppendLine("(");
			stringBuilder.AppendLine("[ArticleSupplierId] ASC");
			stringBuilder.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
			stringBuilder.AppendLine(") ON [PRIMARY]");

			stringBuilder.AppendLine("ALTER TABLE [CRM].[ArticleSupplier] ADD  CONSTRAINT [DF_ArticleSupplier_Favorite]  DEFAULT ((0)) FOR [Favorite]");
			stringBuilder.AppendLine("ALTER TABLE [CRM].[ArticleSupplier] ADD  CONSTRAINT [DF_ArticleSupplier_SortOrder]  DEFAULT ((0)) FOR [SortOrder]");
			stringBuilder.AppendLine("END");

			Database.ExecuteNonQuery(stringBuilder.ToString());
		}
		public override void Down()
		{

		}
	}
}
