namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120101010000)]
	public class CrmArticle : Migration
	{
		public override void Up()
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine("IF NOT EXISTS(select * from sys.tables");
			stringBuilder.AppendLine("where name = 'Article'");
			stringBuilder.AppendLine("and schema_id = schema_id('CRM'))");
			stringBuilder.AppendLine("BEGIN");
			stringBuilder.AppendLine("CREATE TABLE [CRM].[Article](");
			stringBuilder.AppendLine("[ArticleId] [int] NOT NULL,");
			stringBuilder.AppendLine("[ItemNo] [nvarchar](50) NOT NULL,");
			stringBuilder.AppendLine("[Description] [nvarchar](150) NOT NULL,");
			stringBuilder.AppendLine("[ArticleType] [nvarchar](10) NOT NULL,");
			stringBuilder.AppendLine("[Price] [money] NULL,");
			stringBuilder.AppendLine("[QuantityUnit] [nvarchar](10) NULL,");
			stringBuilder.AppendLine("[IsSerial] [int] NOT NULL,");
			stringBuilder.AppendLine("[Weight] [real] NULL,");
			stringBuilder.AppendLine("[WeightNet] [real] NULL,");
			stringBuilder.AppendLine("[Length] [real] NULL,");
			stringBuilder.AppendLine("[Width] [real] NULL,");
			stringBuilder.AppendLine("[Height] [real] NULL,");
			stringBuilder.AppendLine("[MatchCode] [nvarchar](50) NULL,");
			stringBuilder.AppendLine("[ArticleGroup01] [nvarchar](20) NULL,");
			stringBuilder.AppendLine("[ArticleGroup02] [nvarchar](20) NULL,");
			stringBuilder.AppendLine("[ArticleGroup03] [nvarchar](20) NULL,");
			stringBuilder.AppendLine("[IsBatch] [int] NOT NULL,");
			stringBuilder.AppendLine("[Remark] [nvarchar](100) NULL,");
			stringBuilder.AppendLine("[MaintenanceIntervalMonths] [int] NULL,");
			stringBuilder.AppendLine("[UserString01] [nvarchar](50) NULL,");
			stringBuilder.AppendLine("[UserString02] [nvarchar](50) NULL,");
			stringBuilder.AppendLine("[UserString03] [nvarchar](50) NULL,");
			stringBuilder.AppendLine("[UserString04] [nvarchar](50) NULL,");
			stringBuilder.AppendLine("[UserString05] [nvarchar](50) NULL,");
			stringBuilder.AppendLine("[UserFlag01] [int] NULL,");
			stringBuilder.AppendLine("[UserFlag02] [int] NULL,");
			stringBuilder.AppendLine("[UserFlag03] [int] NULL,");
			stringBuilder.AppendLine("[UserFlag04] [int] NULL,");
			stringBuilder.AppendLine("[UserFlag05] [int] NULL,");
			stringBuilder.AppendLine("[UserDate01] [datetime] NULL,");
			stringBuilder.AppendLine("[UserDate02] [datetime] NULL,");
			stringBuilder.AppendLine("[UserDate03] [datetime] NULL,");
			stringBuilder.AppendLine("[UserReal01] [real] NULL,");
			stringBuilder.AppendLine("[UserReal02] [real] NULL,");
			stringBuilder.AppendLine("[UserReal03] [real] NULL,");
			stringBuilder.AppendLine("[DangerousGoodsFlag] [int] NOT NULL,");
			stringBuilder.AppendLine("[MinimumStock] [real] NULL,");
			stringBuilder.AppendLine("[Barcode] [nvarchar](50) NULL,");
			stringBuilder.AppendLine("[IsSparePart] [bit] NULL,");
			stringBuilder.AppendLine("[Cost] [money] NULL,");
			stringBuilder.AppendLine("CONSTRAINT [PK_Article] PRIMARY KEY CLUSTERED");
			stringBuilder.AppendLine("(");
			stringBuilder.AppendLine("[ArticleId] ASC");
			stringBuilder.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
			stringBuilder.AppendLine(") ON [PRIMARY]");
			stringBuilder.AppendLine("END");

			Database.ExecuteNonQuery(stringBuilder.ToString());
		}
		public override void Down()
		{

		}
	}
}
