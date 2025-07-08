using Crm.DynamicForms.Model;
using Crm.Library.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Crm.DynamicForms.ModelBinders
{
	public class DynamicFormFileResponseModelBinder : CrmModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var result = base.BindModel(controllerContext, bindingContext) as DynamicFormFileResponse;
			return result;
		}
	}
}