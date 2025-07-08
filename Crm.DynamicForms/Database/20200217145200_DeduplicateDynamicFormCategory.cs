namespace Crm.DynamicForms.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200217145200)]
	public class DeduplicateDynamicFormCategory : Migration
	{
		public override void Up()
		{
			const string query = @"FROM [LU].[DynamicFormCategory] dfc1, [LU].[DynamicFormCategory] dfc2
				WHERE dfc1.DynamicFormCategoryId > dfc2.DynamicFormCategoryId AND dfc1.[Language] = dfc2.[Language] and dfc1.[Value] = dfc2.[Value]";

			if ((int)Database.ExecuteScalar($"SELECT COUNT(dfc1.[Value]) {query}") > 0)
			{
				Database.ExecuteNonQuery($"DELETE dfc1 {query}");
			}
		}
	}
}