namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200303091000)]
	public class AddFileResourceIdToDynamicFormElement : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("CRM.DynamicFormElement", "FileResourceId"))
			{
				Database.AddColumn("CRM.DynamicFormElement", new Column("FileResourceId", DbType.Guid, ColumnProperty.Null));
				Database.AddForeignKey("FK_DynamicFormElement_FileResource", "CRM.DynamicFormElement", "FileResourceId", "CRM.FileResource", "Id");
			}
		}
	}
}