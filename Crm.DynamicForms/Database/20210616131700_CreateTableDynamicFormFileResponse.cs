using Crm.Library.Data.MigratorDotNet.Framework;
using System.Data;

namespace Crm.DynamicForms.Database
{

	[Migration(20210616131700)]
	public class CreateTableDynamicFormFileResponse : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[CRM].[DynamicFormFileResponse]"))
			{
				Database.AddTable("CRM.DynamicFormFileResponse",
					new Column("DynamicFormFileResponseId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("DynamicFormReferenceKey", DbType.Guid, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 20, ColumnProperty.NotNull),
					new Column("FileResourceId", DbType.Guid, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'")
					);


				if (!Database.ConstraintExists("[CRM].[DynamicFormFileResponse]", "FK_DynamicFormFileResponse_DynamicFormReference"))
				{
					Database.AddForeignKey("FK_DynamicFormFileResponse_DynamicFormReference", "[CRM].[DynamicFormFileResponse]", "DynamicFormReferenceKey", "[CRM].[DynamicFormReference]", "DynamicFormReferenceId");
				}

				if (!Database.ConstraintExists("[CRM].[DynamicFormFileResponse]", "FK_DynamicFormFileResponse_FileResource"))
				{
					Database.AddForeignKey("FK_DynamicFormFileResponse_FileResource", "[CRM].[DynamicFormFileResponse]", "FileResourceId", "[CRM].[FileResource]", "Id");
				}
			}
		}
	}
}