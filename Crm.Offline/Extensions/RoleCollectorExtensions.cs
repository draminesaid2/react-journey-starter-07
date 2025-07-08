namespace Crm.Offline.Extensions
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using System.Linq;

	public static class RoleCollectorExtensions
	{
		public static void AddSyncPermission(this RoleCollectorBase roleCollector, string entityName, params string[] roleNames)
		{
			roleCollector.Add(PermissionGroup.Sync, entityName, roleNames);
			roleCollector.AddImport(PermissionGroup.Sync, entityName, PermissionGroup.WebApi, entityName);
		}
	}
}