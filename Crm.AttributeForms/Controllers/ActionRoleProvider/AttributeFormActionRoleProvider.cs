namespace Crm.AttributeForms.Controllers.ActionRoleProvider
{
	using Crm.AttributeForms.Model;
	using Crm.DynamicForms;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Service;

	public class AttributeFormActionRoleProvider : RoleCollectorBase
	{
		public AttributeFormActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			var leadRoles = new []
			{
				MainPlugin.Roles.SalesBackOffice,
				MainPlugin.Roles.HeadOfSales,
				MainPlugin.Roles.InternalSales,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService
			};
			var roles = new[]
			{
				MainPlugin.Roles.SalesBackOffice,
				MainPlugin.Roles.HeadOfSales,
				MainPlugin.Roles.InternalSales,
				MainPlugin.Roles.FieldSales,
				ServicePlugin.Roles.ServiceBackOffice,
				ServicePlugin.Roles.HeadOfService,
				ServicePlugin.Roles.InternalService,
				ServicePlugin.Roles.FieldService
			};

			Add(PermissionGroup.WebApi, nameof(AttributeForm), roles);

			Add(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Index, leadRoles);
			Add(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Read, leadRoles);
			Add(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Create, leadRoles);
			Add(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Edit, leadRoles);
			Add(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Delete, leadRoles);

			Add(nameof(AttributeForm), PermissionName.Read, roles);
			Add(nameof(AttributeForm), PermissionName.Edit, roles);
			AddImport(nameof(AttributeForm), PermissionName.Edit, nameof(AttributeForm), PermissionName.Read);
			Add(nameof(AttributeForm), PermissionName.Create, leadRoles);
			AddImport(nameof(AttributeForm), PermissionName.Create, nameof(AttributeForm), PermissionName.Edit);
		}
	}
}
