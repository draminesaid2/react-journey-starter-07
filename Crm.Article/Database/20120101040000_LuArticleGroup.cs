namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120101040000)]
	public class LuArticleGroup : Migration
	{
		public override void Up()
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine("IF NOT EXISTS(select * from sys.tables");
			stringBuilder.AppendLine("where name = 'ArticleGroup'");
			stringBuilder.AppendLine("and schema_id = schema_id('LU'))");
			stringBuilder.AppendLine("BEGIN");
			stringBuilder.AppendLine("CREATE TABLE [LU].[ArticleGroup](");
			stringBuilder.AppendLine("[ArticleGroupId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,");
			stringBuilder.AppendLine("[Name] [nvarchar](150) NOT NULL,");
			stringBuilder.AppendLine("[Language] [char](2) NULL,");
			stringBuilder.AppendLine("[Value] [nvarchar](50) NOT NULL,");
			stringBuilder.AppendLine("[Favorite] [bit] NOT NULL,");
			stringBuilder.AppendLine("[SortOrder] [int] NOT NULL,");
			stringBuilder.AppendLine("[Remark] [nvarchar](250) NULL,");
			stringBuilder.AppendLine("CONSTRAINT [PK_ArticleGroup] PRIMARY KEY CLUSTERED ");
			stringBuilder.AppendLine("(");
			stringBuilder.AppendLine("[ArticleGroupId] ASC");
			stringBuilder.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
			stringBuilder.AppendLine(") ON [PRIMARY]");

			stringBuilder.AppendLine("ALTER TABLE [LU].[ArticleGroup] ADD  CONSTRAINT [DF_ArticleGroup_Language]  DEFAULT ('en') FOR [Language]");
			stringBuilder.AppendLine("ALTER TABLE [LU].[ArticleGroup] ADD  CONSTRAINT [DF_ArticleGroup_Value]  DEFAULT ((0)) FOR [Value]");
			stringBuilder.AppendLine("ALTER TABLE [LU].[ArticleGroup] ADD  CONSTRAINT [DF_ArticleGroup_Favorite]  DEFAULT ((0)) FOR [Favorite]");
			stringBuilder.AppendLine("ALTER TABLE [LU].[ArticleGroup] ADD  CONSTRAINT [DF_ArticleGroup_SortOrder]  DEFAULT ((0)) FOR [SortOrder]");
			stringBuilder.AppendLine("END");

			Database.ExecuteNonQuery(stringBuilder.ToString());
		}
		public override void Down()
		{

		}
	}
}
