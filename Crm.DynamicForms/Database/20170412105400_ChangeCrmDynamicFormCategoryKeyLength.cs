namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170412105400)]
	public class ChangeCrmDynamicFormCategoryKeyLength : Migration
	{
		public override void Up()
		{
			Database.ChangeColumn("[CRM].[DynamicForm]", new Column("CategoryKey", DbType.String, 100, ColumnProperty.NotNull));
		}
	}
}