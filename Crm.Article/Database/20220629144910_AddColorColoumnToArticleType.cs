namespace Crm.Database.Modify
{
	using System.Text;
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220629144910)]
	public class AddColorColoumnToArticleType : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[LU].[ArticleType]") && !Database.ColumnExists("[LU].[ArticleType]", "Color"))
			{
				StringBuilder query = new StringBuilder();
				query.Append("ALTER TABLE [LU].[ArticleType] ADD Color nvarchar(20) NOT NULL DEFAULT '#AAAAAA'");
				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}
