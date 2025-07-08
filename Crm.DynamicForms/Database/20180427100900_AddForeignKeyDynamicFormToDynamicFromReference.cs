namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180427100900)]
	public class AddForeignKeyDynamicFormToDynamicFromReference : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = '[FK_DynamicFormReference_DynamicForm]' OR (parent_object_id = OBJECT_ID(N'[CRM].[DynamicFormReference]') AND referenced_object_id = OBJECT_ID(N'[CRM].[DynamicForm]')))
				ALTER TABLE [CRM].[DynamicFormReference]  WITH CHECK ADD  CONSTRAINT [FK_DynamicFormReference_DynamicForm] FOREIGN KEY([DynamicFormKey])
				REFERENCES [CRM].[DynamicForm] ([DynamicFormId])
				ALTER TABLE [CRM].[DynamicFormReference] CHECK CONSTRAINT [FK_DynamicFormReference_DynamicForm]");
		}
	}
}