namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120101060000)]
	public class LuQuantityUnit : Migration
	{
		public override void Up()
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine("IF NOT EXISTS(select * from sys.tables");
			stringBuilder.AppendLine("where name = 'QuantityUnit'");
			stringBuilder.AppendLine("and schema_id = schema_id('LU'))");
			stringBuilder.AppendLine("BEGIN");
			stringBuilder.AppendLine("CREATE TABLE [LU].[QuantityUnit](");
			stringBuilder.AppendLine("[QuantityUnitId] [int] IDENTITY(1,1) NOT NULL,");
			stringBuilder.AppendLine("[Name] [nvarchar](20) NOT NULL,");
			stringBuilder.AppendLine("[Language] [char](2) NULL,");
			stringBuilder.AppendLine("[Value] [nvarchar](10) NOT NULL,");
			stringBuilder.AppendLine("[Favorite] [bit] NOT NULL,");
			stringBuilder.AppendLine("[SortOrder] [int] NOT NULL,");
			stringBuilder.AppendLine("CONSTRAINT [PK_QuantityUnit] PRIMARY KEY CLUSTERED ");
			stringBuilder.AppendLine("(");
			stringBuilder.AppendLine("[QuantityUnitId] ASC");
			stringBuilder.AppendLine(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");
			stringBuilder.AppendLine(") ON [PRIMARY]");

			stringBuilder.AppendLine("ALTER TABLE [LU].[QuantityUnit] ADD  CONSTRAINT [DF_QuantityUnit_Language]  DEFAULT ('en') FOR [Language]");
			stringBuilder.AppendLine("ALTER TABLE [LU].[QuantityUnit] ADD  CONSTRAINT [DF_QuantityUnit_Value]  DEFAULT ((0)) FOR [Value]");
			stringBuilder.AppendLine("ALTER TABLE [LU].[QuantityUnit] ADD  CONSTRAINT [DF_QuantityUnit_Favorite]  DEFAULT ((0)) FOR [Favorite]");
			stringBuilder.AppendLine("ALTER TABLE [LU].[QuantityUnit] ADD  CONSTRAINT [DF_QuantityUnit_SortOrder]  DEFAULT ((0)) FOR [SortOrder]");
			stringBuilder.AppendLine("END");

			Database.ExecuteNonQuery(stringBuilder.ToString());
		}
		public override void Down()
		{

		}
	}
}
