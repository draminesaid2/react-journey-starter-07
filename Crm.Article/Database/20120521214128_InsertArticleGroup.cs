namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120521214128)]
	public class InsertArticleGroup : Migration
	{
		public override void Up()
		{
			var sb = new StringBuilder();

			var doesSmsArticleGroupExist = Database.TableExists("[SMS].[ArticleGroup]");
			var oldArticleGroupCount = doesSmsArticleGroupExist
			                           	? (int)Database.ExecuteScalar("SELECT COUNT(*) FROM [SMS].[ArticleGroup]")
			                           	: 0;

			if (doesSmsArticleGroupExist && oldArticleGroupCount > 0)
			{
				sb.AppendLine("INSERT INTO [LU].[ArticleGroup] (Name, Language, Value)");
				sb.AppendLine("SELECT Name, Language, Value");
				sb.AppendLine("FROM [SMS].[ArticleGroup]");
				Database.ExecuteNonQuery(sb.ToString());
			}

			if (doesSmsArticleGroupExist)
			{
				Database.ExecuteNonQuery("sp_rename 'SMS.ArticleGroup', 'Old_ArticleGroup'");
			}
		}
		public override void Down()
		{
		}
	}
}