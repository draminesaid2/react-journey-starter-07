﻿namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class GenericListActionRoleProvider : RoleCollectorBase
	{
		public GenericListActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var genericListTypes = new[]
			{
				MainPlugin.PermissionGroup.Company,
				MainPlugin.PermissionGroup.Person,
				MainPlugin.PermissionGroup.Task
			};

			foreach (var permissionGroup in genericListTypes)
			{
				Add(permissionGroup, MainPlugin.PermissionName.ExportAsCsv, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
				Add(permissionGroup, MainPlugin.PermissionName.Ics, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
				Add(permissionGroup, MainPlugin.PermissionName.Rss, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			}
		}
	}
}