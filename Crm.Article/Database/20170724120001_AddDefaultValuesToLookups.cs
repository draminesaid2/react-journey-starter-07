namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
	using Crm.Library.Extensions;

	[Migration(20170724120001)]
	public class AddDefaultValuesToLookups : Migration
	{
		public override void Up()
		{
			new[]
			{
				new { Schema = "LU", Table = "ArticleDescription" },
				new { Schema = "LU", Table = "ArticleGroup" },
				new { Schema = "LU", Table = "ArticleGroup2" },
				new { Schema = "LU", Table = "ArticleGroup3" },
				new { Schema = "LU", Table = "ArticleGroup4" },
				new { Schema = "LU", Table = "ArticleGroup5" },
				new { Schema = "LU", Table = "ArticleRelationshipType" },
				new { Schema = "LU", Table = "ArticleType" },
				new { Schema = "LU", Table = "QuantityUnit" },
				new { Schema = "LU", Table = "VATLevel" }
			}
			.ForEach(x => Database.AddEntityBaseDefaultContraints(x.Schema, x.Table));
		}
	}
}