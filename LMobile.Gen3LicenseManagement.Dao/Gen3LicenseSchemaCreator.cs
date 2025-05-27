using System;
using System.Data.Common;
using System.Data.SqlClient;
using LMobile.Gida;

namespace LMobile.Gen3LicenseManagement.Dao {
	public class Gen3LicenseSchemaCreator {
		public static void CreateSchema() {
			var masterConnector = GidaConfiguration.Connectors["LicenseManagementMaster"];
			var connector = GidaConfiguration.Connectors["LicenseManagement"];
			var builder = connector.GetConnectionStringBuilder();
			var databaseName = ((SqlConnectionStringBuilder)builder).InitialCatalog;
			masterConnector.GetSchemaCreator().CreateDatabase(databaseName);
			masterConnector = masterConnector.CloneUsingDatabase(databaseName);
			var mappingFactory = new Gen3LicenseManagementFactory(masterConnector);
				mappingFactory.CreateAllTables();
			var schemaCreator = masterConnector.GetSchemaCreator();
			//schemaCreator.CreateNumberingSequence("ODC", "JournalID", 1, 1);
			var user = CreateUser(schemaCreator, builder);
			if (user != null) {
				mappingFactory.GrantReadOnAllTables(user);
				mappingFactory.GrantWriteOnAllTables(user);
				//schemaCreator.GrantSequence("ODC", "JournalID", odcUser);
			}
		}
		private static string CreateUser(GidaSchemaCreator schemaCreator, DbConnectionStringBuilder builder) {
			string user, password;
			if (builder is SqlConnectionStringBuilder) {
				user = ((SqlConnectionStringBuilder)builder).UserID;
				password = ((SqlConnectionStringBuilder)builder).Password;
			} else {
				throw new InvalidOperationException();
			}
			if (!String.IsNullOrEmpty(user)) {
				if (!String.IsNullOrEmpty(password)) {
					schemaCreator.CreateLogin(user, password);
				}
				return schemaCreator.GetOrCreateUserForLogin(user, user);
			} else {
				return null;
			}
		}
	}
}

