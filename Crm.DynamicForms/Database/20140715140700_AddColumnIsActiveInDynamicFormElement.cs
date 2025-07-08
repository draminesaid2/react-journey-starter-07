namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140715140700)]
	public class AddColumnIsActiveInDynamicFormElement:Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.DynamicFormElement",new Column("IsActive",DbType.Int16,ColumnProperty.Null,1));
		}
	}
}