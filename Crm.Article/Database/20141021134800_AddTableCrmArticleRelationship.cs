namespace Crm.Article.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20141021134800)]
	public class AddTableCrmArticleRelationship : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[CRM].[ArticleRelationship]"))
			{
				var hasArticleIdChangedToGuid = (int)Database.ExecuteScalar("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'Crm' AND TABLE_NAME = 'Article' AND COLUMN_NAME = 'ArticleId' and DATA_TYPE = 'uniqueidentifier'") == 1;
				Database.AddTable("[CRM].[ArticleRelationship]",
					new Column("ArticleRelationshipId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("ParentArticleKey", hasArticleIdChangedToGuid ? DbType.Guid : DbType.Int32, ColumnProperty.NotNull),
					new Column("ChildArticleKey", hasArticleIdChangedToGuid ? DbType.Guid : DbType.Int32, 50, ColumnProperty.NotNull),
					new Column("RelationshipType", DbType.String, 20, ColumnProperty.NotNull),
					new Column("QuantityValue", DbType.Decimal, ColumnProperty.Null),
					new Column("QuantityUnit", DbType.String, 10, ColumnProperty.Null),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, 50, ColumnProperty.NotNull),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, 50, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("TenantKey", DbType.Int32, ColumnProperty.Null));
			}

			if (!Database.TableExists("[LU].[ArticleRelationshipType]"))
			{
				Database.AddTable("[LU].[ArticleRelationshipType]",
					new Column("ArticleRelationshipTypeId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 20, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("InverseName", DbType.String, ColumnProperty.NotNull),
					new Column("ArticleTypes", DbType.String, ColumnProperty.Null),
					new Column("HasQuantity", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull),
					new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("TenantKey", DbType.Int32, ColumnProperty.Null));
				InsertLookupValue("Accessory", "Zubehör", "Zubehör von", "de");
				InsertLookupValue("Accessory", "Accessory", "Accessory of", "en");
				InsertLookupValue("Set", "Set", "Verwendet in Set", "de", "Set", true);
				InsertLookupValue("Set", "Set", "Used in set", "en", "Set", true);
				Database.ExecuteNonQuery(@"INSERT INTO  [LU].[ArticleType]
																		 ([Name]
																		 ,[Language]
																		 ,[Value]
																		 ,[Favorite]
																		 ,[SortOrder]
																		 ,[TenantKey])
															 VALUES
																		 ('Setartikel'
																		 ,'de'
																		 ,'Set'
																		 ,0
																		 ,0
																		 ,null)");
				Database.ExecuteNonQuery(@"INSERT INTO [LU].[ArticleType]
																		 ([Name]
																		 ,[Language]
																		 ,[Value]
																		 ,[Favorite]
																		 ,[SortOrder]
																		 ,[TenantKey])
															 VALUES
																		 ('Set item'
																		 ,'en'
																		 ,'Set'
																		 ,0
																		 ,0
																		 ,null)");
			}
		}
		private void InsertLookupValue(string value, string name, string inverseName, string language, string articleTypes = null, bool hasQuantity = false)
		{
			Database.ExecuteNonQuery(String.Format("INSERT INTO [LU].[ArticleRelationshipType] " +
															 "([Value], [Name], [InverseName], [Language], [ArticleTypes], [HasQuantity], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive])" +
															 "VALUES ('{0}', '{1}', '{2}', '{3}', {4}, '{5}', '0', '0', GETUTCDATE(), GETUTCDATE(), 'Setup', 'Setup', '1')",
															 value, name, inverseName, language, String.IsNullOrWhiteSpace(articleTypes) ? "NULL" : "'" + articleTypes + "'", hasQuantity ? "1" : "0"));
		}
	}
}