namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class TaskActionRoleProvider : RoleCollectorBase
	{
		public TaskActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.Task, PermissionName.View, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Task, PermissionName.Index, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Task, PermissionName.Read, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Task, PermissionName.Create, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Task, PermissionName.Create, MainPlugin.PermissionGroup.Task, MainPlugin.PermissionName.Complete);
			AddImport(MainPlugin.PermissionGroup.Task, PermissionName.Create, MainPlugin.PermissionGroup.Task, PermissionName.Index);
			Add(MainPlugin.PermissionGroup.Task, PermissionName.Edit, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Task, PermissionName.Edit, MainPlugin.PermissionGroup.Task, PermissionName.Create);

			Add(MainPlugin.PermissionGroup.Task, MainPlugin.PermissionName.Ics, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Task, MainPlugin.PermissionName.Ics, MainPlugin.PermissionGroup.Task, PermissionName.Index);
			Add(MainPlugin.PermissionGroup.Task, MainPlugin.PermissionName.Complete, MainPlugin.Roles.FieldSales, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.InternalSales, MainPlugin.Roles.SalesBackOffice);
			Add(MainPlugin.PermissionGroup.Task, MainPlugin.PermissionName.SeeAllUsersTasks, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice);
			AddImport(MainPlugin.PermissionGroup.Task, MainPlugin.PermissionName.SeeAllUsersTasks, MainPlugin.PermissionGroup.Task, PermissionName.Index);
		}
	}
}