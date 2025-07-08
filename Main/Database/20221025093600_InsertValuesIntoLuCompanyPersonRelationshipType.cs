namespace Crm.Database
{

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20221025093600)]

	public class InsertValuesIntoLuCompanyPersonRelationshipType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("INSERT INTO LU.CompanyPersonRelationshipType " +
									 "([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
															 "VALUES ('Other', 'Other', 'en', 0, 0, GETUTCDATE(),GETUTCDATE(),N'Migration_20221025093600', N'Migration_20221025093600',1)");
			Database.ExecuteNonQuery("INSERT INTO LU.CompanyPersonRelationshipType " +
									"([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
															"VALUES ('Other', 'Sonstiges', 'de', 0, 0, GETUTCDATE(),GETUTCDATE(),N'Migration_20221025093600', N'Migration_20221025093600',1)");
			Database.ExecuteNonQuery("INSERT INTO LU.CompanyPersonRelationshipType " +
									"([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
															"VALUES ('Other', 'Autre', 'fr', 0, 0, GETUTCDATE(),GETUTCDATE(),N'Migration_20221025093600', N'Migration_20221025093600',1)");
			Database.ExecuteNonQuery("INSERT INTO LU.CompanyPersonRelationshipType " +
									"([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
															"VALUES ('Other', 'Otra', 'es', 0, 0, GETUTCDATE(),GETUTCDATE(),N'Migration_20221025093600', N'Migration_20221025093600',1)");
			Database.ExecuteNonQuery("INSERT INTO LU.CompanyPersonRelationshipType " +
									 "([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
															 "VALUES ('Consultancy', 'Consultancy', 'en', 0, 0, GETUTCDATE(),GETUTCDATE(),N'Migration_20221025093600', N'Migration_20221025093600',1)");
			Database.ExecuteNonQuery("INSERT INTO LU.CompanyPersonRelationshipType " +
									"([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
															"VALUES ('Consultancy', 'Beratung', 'de', 0, 0, GETUTCDATE(),GETUTCDATE(),N'Migration_20221025093600', N'Migration_20221025093600',1)");
			Database.ExecuteNonQuery("INSERT INTO LU.CompanyPersonRelationshipType " +
									"([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
															"VALUES ('Consultancy', 'Conseil', 'fr', 0, 0, GETUTCDATE(),GETUTCDATE(),N'Migration_20221025093600', N'Migration_20221025093600',1)");
			Database.ExecuteNonQuery("INSERT INTO LU.CompanyPersonRelationshipType " +
									"([Value], [Name], [Language], [Favorite], [SortOrder], [CreateDate], [ModifyDate], [CreateUser], [ModifyUser], [IsActive]) " +
															"VALUES ('Consultancy', 'Consultoría', 'es', 0, 0, GETUTCDATE(),GETUTCDATE(),N'Migration_20221025093600', N'Migration_20221025093600',1)");
		}
	}
}
