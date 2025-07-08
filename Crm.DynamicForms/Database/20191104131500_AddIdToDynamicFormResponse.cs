namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20191104131500)]
	public class AddIdToDynamicFormResponse : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF EXISTS (SELECT * FROM sys.key_constraints WHERE type = 'PK' AND OBJECT_NAME(parent_object_id) = N'DynamicFormResponse')
				BEGIN
					ALTER TABLE [CRM].[DynamicFormResponse]
					DROP CONSTRAINT PK_DynamicFormResponse;
				END");
			Database.AddColumnIfNotExisting("[CRM].[DynamicFormResponse]", new Column("DynamicFormResponseId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"));
		}
	}
}
