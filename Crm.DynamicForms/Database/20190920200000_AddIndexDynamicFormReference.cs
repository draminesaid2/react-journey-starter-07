namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190920200000)]
	public class AddIndexDynamicFormReference : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[DynamicFormReference]') AND name = N'IX_DynamicFormReference_Type_IsActive_ReferenceKey')
			BEGIN
				CREATE NONCLUSTERED INDEX IX_DynamicFormReference_Type_IsActive_ReferenceKey
				ON [CRM].[DynamicFormReference] ([DynamicFormReferenceType],[IsActive],[ReferenceKey])
				INCLUDE ([DynamicFormReferenceId])
			END");
		}
	}
}
