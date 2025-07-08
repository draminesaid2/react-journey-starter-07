using System;

namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	[Migration(20140716133600)]
	public class AlterDynamicFormElementMakeTitleMax:Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("CRM.DynamicFormElement",new Column("Title",DbType.String,Int32.MaxValue,ColumnProperty.Null));
		}
	}
}