namespace Crm.AttributeForms
{
	using Crm.Library.Configuration;
	using Crm.Library.Modularization;

	[Plugin(ModuleId = "FLD03130", Requires = "Main,Crm.DynamicForms")]
	public class AttributeFormsPlugin : Plugin
	{
		public new const string Name = "Crm.AttributeForms";


		public static class Settings
		{
			public static SettingDefinition<string[]> ExcludedContactTypes => new SettingDefinition<string[]>("ExcludedContactTypes", Name);
			public static SettingDefinition<int> AttributeFormsCountPerContact => new SettingDefinition<int>("AttributeFormsCountPerContact", Name);
		}
	}
}
