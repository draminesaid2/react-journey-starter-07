namespace Crm.DynamicForms
{
	using Crm.Library.Configuration;
	using Crm.Library.Modularization;

	[Plugin(Requires = "Main")]
	public class DynamicFormsPlugin : Plugin
	{
		public static readonly string PluginName = typeof(DynamicFormsPlugin).Namespace;

		public static class PermissionGroup
		{
			public const string DynamicForms = "DynamicForms";
			public const string PdfFeature = "PdfFeature";
		}

		public static class Settings
		{
			public static class DynamicFormElement
			{
				public static SettingDefinition<bool> ShowPrivacyPolicy => new SettingDefinition<bool>("DynamicFormElement/SignaturePad/Show/PrivacyPolicy", PluginName);
			}
		}
	}
}