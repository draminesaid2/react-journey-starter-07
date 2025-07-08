namespace Crm.Offline.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;

	[Migration(20190722083200)]
	public class MigrateWebApiPermissions : Migration
	{
		public override void Up()
		{
			var permissionsMigratedByUnicore = (int)Database.ExecuteScalar($"SELECT COUNT(*) FROM [dbo].[Permission] WHERE [Name] = '{PermissionGroup.Offline}::OfflineAccess'") == 1;
			if (!permissionsMigratedByUnicore)
			{
				Database.ExecuteNonQuery("UPDATE [dbo].[Permission] SET [Name] = REPLACE([Name], 'WebApi::', 'Sync::'), [Group] = 'Crm.Offline' WHERE [Name] LIKE 'WebApi::%'");
			}
			
		}
	}
}
