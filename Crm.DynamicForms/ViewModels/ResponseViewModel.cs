namespace Crm.DynamicForms.ViewModels
{
	using Crm.DynamicForms.Model;
	using Crm.Library.AutoFac;
	
	public interface IResponseViewModel<TDynamicFormReference> : IResponseViewModel
		where TDynamicFormReference : DynamicFormReference
	{
		TDynamicFormReference DynamicFormReference { get; set; }
	}
	public interface IResponseViewModel : ITransientDependency
	{
		double HeaderContentSize { get; }
		double HeaderContentSpacing { get; }
		double FooterContentSize { get; }
		double FooterContentSpacing { get; }
	}
}