namespace Crm.DynamicForms.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170823102100)]
	public class ChangeLuDynamicFormLocalizationNameLength : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("LU.DynamicFormLocalization", new Column("Name", DbType.String, Int32.MaxValue, ColumnProperty.NotNull));
		}
	}
}