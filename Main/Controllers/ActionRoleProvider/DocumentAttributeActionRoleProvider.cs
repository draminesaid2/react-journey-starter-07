namespace Crm.Controllers.ActionRoleProvider
{
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class DocumentAttributeActionRoleProvider : RoleCollectorBase
	{
		public DocumentAttributeActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(MainPlugin.PermissionGroup.DocumentAttribute, PermissionName.View, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			Add(MainPlugin.PermissionGroup.DocumentAttribute, PermissionName.Create, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			Add(MainPlugin.PermissionGroup.DocumentAttribute, PermissionName.Index, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.InternalSales, MainPlugin.Roles.FieldSales);
			AddImport(MainPlugin.PermissionGroup.DocumentAttribute, PermissionName.Index, PermissionGroup.File, PermissionName.GetContent);
			AddImport(MainPlugin.PermissionGroup.DocumentAttribute, PermissionName.Index, PermissionGroup.File, PermissionName.ThumbnailImage);
			Add(MainPlugin.PermissionGroup.DocumentAttribute, PermissionName.Delete, MainPlugin.Roles.HeadOfSales, MainPlugin.Roles.SalesBackOffice, MainPlugin.Roles.FieldSales);
			AddImport(MainPlugin.PermissionGroup.DocumentAttribute, PermissionName.Delete, MainPlugin.PermissionGroup.DocumentArchive, PermissionName.Create);
		}
	}
}