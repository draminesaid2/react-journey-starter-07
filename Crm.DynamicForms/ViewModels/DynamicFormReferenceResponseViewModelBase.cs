namespace Crm.DynamicForms.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.BaseModel;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.Library.BaseModel;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Model.Site;
	using Crm.Library.ViewModels;
	using Crm.Services.Interfaces;

	public class DynamicFormReferenceResponseViewModelBase : HtmlTemplateViewModel
	{
		public DynamicFormReferenceResponseViewModelBase(DynamicFormReference dynamicFormReference, IAppSettingsProvider appSettingsProvider, IMapper mapper, ISiteService siteService)
		{
			FooterContentSize = appSettingsProvider.GetValue(MainPlugin.Settings.Report.FooterHeight);
			FooterContentSpacing = appSettingsProvider.GetValue(MainPlugin.Settings.Report.FooterMargin);
			HeaderContentSize = appSettingsProvider.GetValue(MainPlugin.Settings.Report.HeaderHeight);
			HeaderContentSpacing = appSettingsProvider.GetValue(MainPlugin.Settings.Report.HeaderMargin);

			var responseOutputViewModels = dynamicFormReference.Responses
				.Where(x => x.DynamicFormElement is IExplicitResponseOutputType && x.ValueAsString != null)
				.OrderByDescending(x => x.ModifyDate)
				.Distinct((x, y) => x.DynamicFormElementKey.Equals(y.DynamicFormElementKey))
				.ToDictionary(k => k.DynamicFormElementKey, v => mapper.Map(v.DynamicFormElement.ParseToClient(v.ValueAsString), v.DynamicFormElement.ParseToClient(v.ValueAsString).GetType(), ((IExplicitResponseOutputType)v.DynamicFormElement).ResponseOutputType));

			var elementOutputViewModels = dynamicFormReference.DynamicForm != null ? dynamicFormReference.DynamicForm.Elements
				.Where(x => x is IExplicitElementOutputType)
				.ToDictionary(k => k.Id, v => mapper.Map(v, v.GetType(), ((IExplicitElementOutputType)v).ElementOutputType))
				: new Dictionary<Guid, object>();

			var responsesOutput = new []{ responseOutputViewModels, elementOutputViewModels }
				.SelectMany(dict => dict)
				.ToLookup(pair => pair.Key, pair => pair.Value)
				.ToDictionary(group => group.Key, group => group.First());

			Responses = dynamicFormReference.Responses.OrderByDescending(x => x.ModifyDate).ToList();
			ResponsesOutput = new SerializableDictionary<Guid, object>(responsesOutput);
			Site = siteService.CurrentSite;
			IsPdfViewModel = true;
			DynamicForm = dynamicFormReference.DynamicForm;
			Id = dynamicFormReference.Id;
			ViewModel = "Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel";
		}
		public DynamicForm DynamicForm { get; set; }
		public DynamicFormElement[] Elements
		{
			get { return DynamicForm?.Elements.ToArray(); }
		}
		public DynamicFormElementRule[] ElementRules
		{
			get { return Elements?.SelectMany(x => x.Rules).ToArray(); }
		}
		public DynamicFormElementRuleCondition[] ElementRuleConditions
		{
			get { return ElementRules?.SelectMany(x => x.Conditions).ToArray(); }
		}
		public string Language => DynamicForm != null && DynamicForm.Languages.Any(x => x.LanguageKey.Equals(Global.CurrentUICulture.TwoLetterISOLanguageName, StringComparison.InvariantCultureIgnoreCase) && x.StatusKey == DynamicFormStatus.ReleasedKey) ? Global.CurrentUICulture.TwoLetterISOLanguageName : DynamicForm?.DefaultLanguageKey;
		public DynamicFormLocalization[] Localizations
		{
			get { return DynamicForm?.Localizations.Where(x => x.Language.Equals(Language)).ToArray(); }
		}
		public IList<DynamicFormResponse> Responses { get; set; }
		public SerializableDictionary<Guid, object> ResponsesOutput { get; set; }
		public Site Site { get; set; }
		public bool IsPdfViewModel { get; set; }
		public virtual double FooterContentSize { get; }
		public virtual double FooterContentSpacing { get; }
		public virtual double HeaderContentSize { get; }
		public virtual double HeaderContentSpacing { get; }
	}
}
