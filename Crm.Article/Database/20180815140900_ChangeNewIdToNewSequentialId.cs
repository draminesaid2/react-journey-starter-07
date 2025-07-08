namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180815140900)]
	public class ChangeNewIdToNewSequentialId : Migration
	{
		public override void Up()
		{
			Database.DropDefault("CRM", "ArticleRelationship", "ArticleRelationshipId");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[ArticleRelationship] ADD CONSTRAINT [DF_ArticleRelationship_ArticleRelationshipId] DEFAULT (newsequentialid()) FOR [ArticleRelationshipId]");
		}
	}
}