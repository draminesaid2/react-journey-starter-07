namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120521214406)]
	public class InsertArticleDescription : Migration
	{
		public override void Up()
		{
			StringBuilder sb = new StringBuilder();

			var doesSmsArticleDescriptionExist = Database.TableExists("[SMS].[ArticleDescription]");
			var oldArticleDescriptionCount = doesSmsArticleDescriptionExist
																		? (int)Database.ExecuteScalar("SELECT COUNT(*) FROM [SMS].[ArticleDescription]")
																		: 0;

			if (doesSmsArticleDescriptionExist && oldArticleDescriptionCount > 0)
			{
				sb.AppendLine("INSERT INTO [LU].[ArticleDescription] (Name, Language, Value)");
				sb.AppendLine("SELECT Name, Language, Value");
				sb.AppendLine("FROM [SMS].[ArticleDescription]");
				Database.ExecuteNonQuery(sb.ToString());
			}
			

			if (doesSmsArticleDescriptionExist)
				Database.ExecuteNonQuery("sp_rename 'SMS.ArticleDescription', 'Old_ArticleDescription'");
		}
		public override void Down()
		{
			
		}
	}
}
