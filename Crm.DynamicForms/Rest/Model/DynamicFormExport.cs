using Crm.Model;

namespace Crm.DynamicForms.Rest.Model
{
	public class DynamicFormExport
	{
		public string Category { get; set; }
		public string Description { get; set; }
		public DynamicFormRest DynamicForm { get; set; }
		public DynamicFormElementRest[] Elements { get; set; }
		public FileResource[] FileResources { get; set; }
		public DynamicFormLanguageRest[] Languages { get; set; }
		public DynamicFormLocalizationRest[] Localizations { get; set; }
		public string Title { get; set; }
		public string[] ImageContents { get; set; }
	}
}
