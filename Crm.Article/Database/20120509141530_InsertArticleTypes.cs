namespace Crm.Article.Database
{
	using System.Text;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20120509141530)]
	public class InsertArticleTypes : Migration
	{
		public override void Up()
		{
			StringBuilder sb = new StringBuilder();

			var doesSmsArticleTypesExist = Database.TableExists("[SMS].[ArticleTypes]");
			var oldArticleTypesCount = doesSmsArticleTypesExist
																		? (int)Database.ExecuteScalar("SELECT COUNT(*) FROM [SMS].[ArticleTypes]")
																		: 0;

			if (doesSmsArticleTypesExist && oldArticleTypesCount > 0)
			{
				sb.AppendLine("INSERT INTO [LU].[ArticleType] (Name, Language, Value)");
				sb.AppendLine("SELECT Name, Language, Value");
				sb.AppendLine("FROM [SMS].[ArticleTypes]");
			}
			else
			{
				sb.AppendLine("INSERT INTO [LU].[ArticleType] (Name, Language, Value)");
				sb.AppendLine("VALUES");
				sb.AppendLine("('Material',				'de',	'Material'),");
				sb.AppendLine("('Material',				'en',	'Material'),");
				sb.AppendLine("('Dienstleistung',	'de',	'Service'),");
				sb.AppendLine("('Service',				'en',	'Service'),");
				sb.AppendLine("('Werkzeug',				'de',	'Tool'),");
				sb.AppendLine("('Tool',						'en',	'Tool')");
			}
			Database.ExecuteNonQuery(sb.ToString());

			if (doesSmsArticleTypesExist)
				Database.ExecuteNonQuery("sp_rename 'SMS.ArticleTypes', 'Old_ArticleTypes'");
		}
		public override void Down()
		{
			
		}
	}
}
