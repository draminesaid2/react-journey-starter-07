namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20201109154500)]
	public class RemoveUnusedLocalizations : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
					UPDATE [LU].[DynamicFormLocalization]
						SET [IsActive] = 0, [ModifyUser] = '20201109154500_RemoveUnusedLocalizations', [ModifyDate] = GETUTCDATE()
						WHERE LEN([Name]) = 0");
		}
	}
}