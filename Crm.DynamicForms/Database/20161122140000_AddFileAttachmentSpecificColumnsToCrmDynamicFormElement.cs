namespace Crm.DynamicForms.Database
{

	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20161122140000)]
	public class AddFileAttachmentSpecificColumnsToCrmDynamicFormElement : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[Crm].[DynamicFormElement]", new Column("MaxImageWidth", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[Crm].[DynamicFormElement]", new Column("MaxImageHeight", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[Crm].[DynamicFormElement]", new Column("MaxFileSize", DbType.Int32, ColumnProperty.Null));
		}
	}
}