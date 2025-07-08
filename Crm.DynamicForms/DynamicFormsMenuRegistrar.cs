namespace Crm.DynamicForms
{
	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.BaseModel;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization.Menu;

	using PermissionGroup = DynamicFormsPlugin.PermissionGroup;

	public class DynamicFormsMenuRegistrar : IMenuRegistrar<MaterialMainMenu>
	{
		public virtual void Initialize(MenuProvider<MaterialMainMenu> menuProvider)
		{
			menuProvider.Register("MasterData", "DynamicForms", url: "~/Crm.DynamicForms/DynamicFormList/IndexTemplate", iconClass: "zmdi zmdi-view-subtitles", priority: 80);
			menuProvider.AddPermission("MasterData", "DynamicForms", PermissionGroup.DynamicForms, PermissionName.View);
			menuProvider.AddPermission("MasterData", "DynamicForms", Library.Model.Authorization.PermissionGroup.WebApi, nameof(DynamicForm));
			menuProvider.AddPermission("MasterData", "DynamicForms", Library.Model.Authorization.PermissionGroup.WebApi, nameof(DynamicFormElement));
		}
	}
}
