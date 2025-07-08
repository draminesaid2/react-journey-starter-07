namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150827112900)]
	public class ChangeCrmDynamicFormReferenceToHighLowIds : Migration
	{
		private const int Low = 32;

		public override void Up()
		{
			object foreignKey;
			do
			{
				foreignKey = Database.ExecuteScalar("SELECT " +
				                                    "'ALTER TABLE [' + s1.name + '].[' + o1.name + '] DROP CONSTRAINT [' + fk.name + ']' AS query " +
				                                    "FROM sys.objects o1 " +
				                                    "INNER JOIN sys.foreign_keys fk " +
				                                    "ON o1.object_id = fk.parent_object_id " +
				                                    "INNER JOIN sys.foreign_key_columns fkc " +
				                                    "ON fk.object_id = fkc.constraint_object_id " +
				                                    "INNER JOIN sys.columns c2 " +
				                                    "ON fkc.referenced_object_id = c2.object_id " +
				                                    "AND fkc.referenced_column_id = c2.column_id " +
				                                    "INNER JOIN sys.objects o2 " +
				                                    "ON fk.referenced_object_id = o2.object_id " +
				                                    "INNER JOIN sys.schemas s1 " +
				                                    "ON o1.schema_id = s1.schema_id " +
				                                    "INNER JOIN sys.schemas s2 " +
				                                    "ON o2.schema_id = s2.schema_id " +
				                                    "WHERE s2.name = 'CRM' AND o2.name = 'DynamicFormReference' AND  c2.name = 'DynamicFormReferenceId'");
				if (foreignKey != null)
				{
					Database.ExecuteNonQuery(foreignKey.ToString());
				}
			}
			while (foreignKey != null);

			var hibernateUniqueKeyTableExists = (int)Database.ExecuteScalar("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'hibernate_unique_key'") > 0;
			var hibernateUniqueKeyTableName = hibernateUniqueKeyTableExists ? "hibernate_unique_key" : "hibernate_unique_key_old";
			Database.ExecuteNonQuery("DECLARE @TableName SYSNAME = 'CRM.DynamicFormReference' " +
			                         "DECLARE @PrimaryKeyName sysname = ( " +
			                         "select name " +
			                         "from sys.key_constraints " +
			                         "where type = 'PK' and parent_object_id = object_id(@TableName)) " +
			                         "EXECUTE ('alter table ' + @TableName + ' drop constraint ' + @PrimaryKeyName)");
			Database.RenameColumn("[CRM].[DynamicFormReference]", "DynamicFormReferenceId", "DynamicFormReferenceId_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DynamicFormReference] ADD DynamicFormReferenceId bigint NULL");
			Database.ExecuteNonQuery("UPDATE [CRM].[DynamicFormReference] SET DynamicFormReferenceId = DynamicFormReferenceId_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DynamicFormReference] ALTER COLUMN DynamicFormReferenceId bigint NOT NULL");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DynamicFormReference] DROP COLUMN DynamicFormReferenceId_Identity");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DynamicFormReference] ADD CONSTRAINT [PK_DynamicFormReference] PRIMARY KEY(DynamicFormReferenceId)");
			Database.ExecuteNonQuery(string.Format("BEGIN IF ((SELECT COUNT(*) FROM dbo.{0} WHERE tablename = '[CRM].[DynamicFormReference]') = 0) INSERT INTO {0} (next_hi, tablename) values ((select (COALESCE(max(DynamicFormReferenceId), 0) / " + Low + ") + 1 from [CRM].[DynamicFormReference] where DynamicFormReferenceId is not null), '[CRM].[DynamicFormReference]') END", hibernateUniqueKeyTableName));

			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DynamicFormResponse] DROP CONSTRAINT [PK_DynamicFormResponse]");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DynamicFormResponse] ALTER COLUMN DynamicFormReferenceKey BIGINT NOT NULL");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[DynamicFormResponse] ADD CONSTRAINT [PK_DynamicFormResponse] PRIMARY KEY CLUSTERED ([DynamicFormReferenceKey] ASC, [DynamicFormElementKey] ASC)");
			Database.AddForeignKey("FK_DynamicFormResponse_DynamicFormReference", "[CRM].[DynamicFormResponse]", "DynamicFormReferenceKey", "[CRM].[DynamicFormReference]", "DynamicFormReferenceId");
		}
	}
}
