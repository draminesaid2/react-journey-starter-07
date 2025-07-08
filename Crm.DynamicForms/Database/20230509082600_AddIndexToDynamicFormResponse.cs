using Crm.Library.Data.MigratorDotNet.Framework;

namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20230509082600)]
	public class AddIndexToDynamicFormResponse : Migration
	{
		public override void Up()
		{
			if (Database.IndexExists("[CRM].[DynamicFormResponse]", "IX_DynamicFormResponse_DynamicFormReferenceKey_IsActive"))
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_DynamicFormResponse_DynamicFormReferenceKey_IsActive] ON [CRM].[DynamicFormResponse]");
			}

			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_DynamicFormResponse_DynamicFormReferenceKey_IsActive] ON [CRM].[DynamicFormResponse] ([DynamicFormReferenceKey] ASC, [IsActive] ASC) INCLUDE ([DynamicFormResponseId])");
		}
	}
}
