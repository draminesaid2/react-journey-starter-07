namespace Crm.Article.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20220222101500)]
	public class ArticleCompanyRelationship : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("CRM.ArticleCompanyRelationship"))
			{
				Database.AddTable("CRM.ArticleCompanyRelationship",
					new Column("RelationshipType", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Information", DbType.String, 150, ColumnProperty.Null),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("ArticleKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("CompanyKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("ArticleCompanyRelationshipId", DbType.Guid, ColumnProperty.PrimaryKey));
				
				Database.ExecuteNonQuery("ALTER TABLE CRM.ArticleCompanyRelationship ADD FOREIGN KEY (ArticleKey) REFERENCES CRM.Article(ArticleId)");
				Database.ExecuteNonQuery("ALTER TABLE CRM.ArticleCompanyRelationship ADD FOREIGN KEY (CompanyKey) REFERENCES CRM.Company(ContactKey)");
			}
			if (!Database.TableExists("LU.ArticleCompanyRelationshipType"))
			{
				Database.AddTable("LU.ArticleCompanyRelationshipType", 
					new Column("ArticleCompanyRelationshipTypeId", DbType.Int16, ColumnProperty.Identity),
					new Column("Name", DbType.String, 20, ColumnProperty.NotNull),
					new Column("Language", DbType.StringFixedLength, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int16, ColumnProperty.Null),
					new Column("Value", DbType.String, 30, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
			}

			Database.InsertLookupValue("ArticleCompanyRelationshipType", "Supplier", "Lieferant", "de");
			Database.InsertLookupValue("ArticleCompanyRelationshipType", "Supplier", "Supplier", "en");
			Database.InsertLookupValue("ArticleCompanyRelationshipType", "Supplier", "Fournisseur", "fr");
			Database.InsertLookupValue("ArticleCompanyRelationshipType", "Supplier", "Proveedor", "es");
			Database.InsertLookupValue("ArticleCompanyRelationshipType", "Supplier", "Támogató", "hu");
		}
	}
}
