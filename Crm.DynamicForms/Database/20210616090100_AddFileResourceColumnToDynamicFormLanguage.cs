
using Crm.Library.Data.MigratorDotNet.Framework;
using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
using System.Data;

namespace Crm.DynamicForms.Database
{
	[Migration(20210616090100)]
	public class AddFileResourceColumnToDynamicFormLanguage : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[CRM].[DynamicFormLanguage]"))
			{
				Database.AddColumnIfNotExisting("[CRM].[DynamicFormLanguage]", new Column("FileResourceId", DbType.Guid, ColumnProperty.Null));
			}


			if (!Database.ConstraintExists("[CRM].[DynamicFormLanguage]", "FK_DynamicFormLanguage_FileResource"))
			{
				Database.AddForeignKey("FK_DynamicFormLanguage_FileResource", "[CRM].[DynamicFormLanguage]", "FileResourceId", "[CRM].[FileResource]", "Id");
			}
		}
	}
}