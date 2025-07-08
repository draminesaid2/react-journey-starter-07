namespace Crm.DynamicForms.Model.Lookups
{
	using System.Globalization;
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[DynamicFormCategory]")]
	public class DynamicFormCategory : EntityLookup<string>
	{
		public static readonly DynamicFormCategory Default = new DynamicFormCategory { Key = string.Empty, Value = "All", Language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName };

		public DynamicFormCategory()
		{
			Key = string.Empty;
			Value = "All";
			Language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
		}
	}
}