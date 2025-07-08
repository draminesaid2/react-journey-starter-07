namespace Crm.Article.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160421110000)]
	public class ArticleDescriptionToEntityLookup : Migration
	{
		public override void Up()
		{
		if (!Database.ColumnExists("LU.ArticleDescription", "CreateUser"))
		{
			Database.ExecuteNonQuery(@"ALTER TABLE LU.ArticleDescription ADD CreateUser nvarchar(100)");
			Database.ExecuteNonQuery(@"UPDATE LU.ArticleDescription SET CreateUser = 'Initial'");
		}
		if (!Database.ColumnExists("LU.ArticleDescription", "ModifyUser"))
		{
			Database.ExecuteNonQuery(@"ALTER TABLE LU.ArticleDescription ADD ModifyUser nvarchar(100)");
			Database.ExecuteNonQuery(@"UPDATE LU.ArticleDescription SET CreateUser = 'Initial'");
		}
		if (!Database.ColumnExists("LU.ArticleDescription", "CreateDate"))
		{
			Database.ExecuteNonQuery(@"ALTER TABLE LU.ArticleDescription ADD CreateDate datetime");
			Database.ExecuteNonQuery(@"UPDATE LU.ArticleDescription SET CreateDate = getutcdate()");
		}
		if (!Database.ColumnExists("LU.ArticleDescription", "ModifyDate"))
		{
			Database.ExecuteNonQuery(@"ALTER TABLE LU.ArticleDescription ADD ModifyDate datetime");
			Database.ExecuteNonQuery(@"UPDATE LU.ArticleDescription SET ModifyDate = getutcdate()");
		}
		if (!Database.ColumnExists("LU.ArticleDescription", "IsActive"))
		{
			Database.ExecuteNonQuery(@"ALTER TABLE LU.ArticleDescription ADD IsActive bit");
			Database.ExecuteNonQuery(@"UPDATE LU.ArticleDescription SET IsActive = 1");
		}
	}
	public override void Down()
		{
		}
	}
}