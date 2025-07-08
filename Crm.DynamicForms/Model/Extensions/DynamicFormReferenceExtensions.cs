namespace Crm.DynamicForms.Model.Extensions
{
	using Crm.Library.Globalization.Resource;

	public static class DynamicFormReferenceExtensions
	{
		public static string GetFileName(this DynamicFormReference dynamicFormReference, IResourceManager resourceManager, string languageKey = null)
		{
			return $"{dynamicFormReference.DynamicForm.GetTitle(languageKey) ?? resourceManager.GetTranslation("DynamicForm")}-{dynamicFormReference.Id}";
		}
	}
}