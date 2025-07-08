namespace Crm.DynamicForms.ViewModels
{
	using AutoMapper;

	using Crm.DynamicForms.Model;
	using Crm.Library.Helper;
	using Crm.Services.Interfaces;

	public class DynamicFormReferenceResponseViewModel : DynamicFormReferenceResponseViewModelBase, IResponseViewModel<DynamicFormReference>
	{
		public DynamicFormReferenceResponseViewModel(DynamicFormReference dynamicFormReference, IAppSettingsProvider appSettingsProvider, IMapper mapper, ISiteService siteService)
			: base(dynamicFormReference, appSettingsProvider, mapper, siteService)
		{
			DynamicFormReference = dynamicFormReference;
		}
		public virtual DynamicFormReference DynamicFormReference { get; set; }
	}
}
