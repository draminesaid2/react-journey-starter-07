using Crm.Library.Model.Authorization;
using Crm.Library.Modularization.Menu;
using Main.Flow.Model;

namespace Main.Flow
{
	public class FlowMenuRegistrar : IMenuRegistrar<MaterialMainMenu>
	{
		public virtual void Initialize(MenuProvider<MaterialMainMenu> menuProvider)
		{
			menuProvider.Register("Administration", "FlowProcessing", url: "~/Main.Flow/FlowItemList/IndexTemplate");
			menuProvider.AddPermission("Administration", "FlowProcessing", PermissionGroup.WebApi, nameof(FlowItem));
			menuProvider.Register("Administration", "FlowRule", url: "~/Main.Flow/FlowRuleList/IndexTemplate");
			menuProvider.AddPermission("Administration", "FlowRule", PermissionGroup.WebApi, nameof(FlowRule));
		}
	}
}
