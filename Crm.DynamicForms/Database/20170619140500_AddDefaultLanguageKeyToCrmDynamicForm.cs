namespace Crm.DynamicForms.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20170619140500)]
	public class AddDefaultLanguageKeyToCrmDynamicForm : Migration
	{
		public override void Up()
		{
			var defaultLanguage = Database.TableExists("[CRM].[Site]") ? (string)Database.ExecuteScalar("SELECT TOP 1 SUBSTRING([DefaultLanguage], 0, 3) FROM [CRM].[Site]") : (string)Database.ExecuteScalar("SELECT TOP 1 SUBSTRING([DefaultLanguageKey], 0, 3) FROM [dbo].[Domain]");
			Database.AddColumnIfNotExisting("[CRM].[DynamicForm]", new Column("DefaultLanguageKey", DbType.String, 2, ColumnProperty.NotNull, $"'{defaultLanguage}'"));
		}
	}
}
