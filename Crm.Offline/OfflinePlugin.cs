namespace Crm.Offline
{
	using Crm.Library.Configuration;
	using Crm.Library.Modularization;
	
	[Plugin(ModuleId = "FLD03000", Requires = "Main,Main.Replication")]
	public class OfflinePlugin : Plugin
	{
		public static string PluginName = typeof(OfflinePlugin).Namespace;

		public static class Settings
		{
			public static SettingDefinition<float> WaitForPostingServiceTimeoutInSec => new SettingDefinition<float>("WaitForPostingServiceTimeoutInSec", PluginName);
		}
	}
}