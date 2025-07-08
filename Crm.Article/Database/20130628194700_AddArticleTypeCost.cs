namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130628194700)]
	public class AddArticleTypeCost : Migration
	{
		public override void Up()
		{
			var count = (int)Database.ExecuteScalar("SELECT COUNT(*) FROM [LU].[ArticleType] WHERE [Value]= 'Cost'");

			if (count == 0)
			{
				var sb = new StringBuilder();
				sb.AppendLine("INSERT INTO [LU].[ArticleType] (Name, Language, Value)");
				sb.AppendLine("VALUES ('Kosten', 'de', 'Cost')");
				sb.AppendLine("INSERT INTO [LU].[ArticleType] (Name, Language, Value)");
				sb.AppendLine("VALUES ('Cost', 'en', 'Cost')");
				Database.ExecuteNonQuery(sb.ToString());
			}

		}
		public override void Down()
		{
			
		}
	}
}
