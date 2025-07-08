namespace Crm.DynamicForms.Controllers.ActionRoleProvider
{
	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.BaseModel;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Interfaces;

	public class DynamicFormsActionRoleProvider : RoleCollectorBase
	{
		public DynamicFormsActionRoleProvider(IPluginProvider pluginProvider)
			: base(pluginProvider)
		{
			Add(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.View);
			Add(DynamicFormsPlugin.PermissionGroup.PdfFeature, PermissionName.Read);
			AddImport(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.View, DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Index);
			AddImport(DynamicFormsPlugin.PermissionGroup.PdfFeature, PermissionName.Read, DynamicFormsPlugin.PermissionGroup.PdfFeature, PermissionName.Index);
			Add(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Create);
			AddImport(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Create, DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Index);
			Add(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Delete);
			AddImport(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Delete, DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Edit);
			Add(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Edit);
			AddImport(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Edit, DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Create);
			Add(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Index);
			Add(DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Read);

			Add(PermissionGroup.WebApi, nameof(DynamicForm), Roles.APIUser);
			AddImport(PermissionGroup.WebApi, nameof(DynamicForm), DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Edit);
			AddImport(PermissionGroup.WebApi, nameof(DynamicForm), DynamicFormsPlugin.PermissionGroup.DynamicForms, PermissionName.Delete);
			Add(PermissionGroup.WebApi, nameof(DynamicFormElement), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DynamicFormElementRule), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DynamicFormElementRuleCondition), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DynamicFormLanguage), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DynamicFormLocalization), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DynamicFormStatus), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DynamicFormCategory), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DynamicFormResponse), Roles.APIUser);
			Add(PermissionGroup.WebApi, nameof(DynamicFormFileResponse), Roles.APIUser);
		}
	}
}
