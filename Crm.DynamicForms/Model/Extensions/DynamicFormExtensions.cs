namespace Crm.DynamicForms.Model.Extensions
{
	using System.Linq;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.Library.Helper;

	public static class DynamicFormExtensions
	{
		public static DynamicFormLocalization GetDynamicFormLocalization(this DynamicForm dynamicForm, string languageKey = null)
		{
			languageKey = languageKey ?? Global.CurrentUICulture.TwoLetterISOLanguageName;
			return dynamicForm.Localizations.FirstOrDefault(x => !x.DynamicFormElementId.HasValue && x.Language.Equals(languageKey)) ?? dynamicForm.Localizations.FirstOrDefault(x => !x.DynamicFormElementId.HasValue && x.Language.Equals(dynamicForm.DefaultLanguageKey));
		}
		public static string GetDescription(this DynamicForm dynamicForm, string languageKey = null)
		{
			return dynamicForm.GetDynamicFormLocalization(languageKey)?.Hint;
		}
		public static string GetTitle(this DynamicForm dynamicForm, string languageKey = null)
		{
			return dynamicForm.GetDynamicFormLocalization(languageKey)?.Value;
		}
	}
}