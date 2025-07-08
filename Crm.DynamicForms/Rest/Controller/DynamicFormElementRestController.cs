using Microsoft.AspNetCore.Mvc;

namespace Crm.DynamicForms.Rest.Controller
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Crm.DynamicForms.Model.BaseModel;
	using Crm.DynamicForms.Services;
	using Crm.Library.AutoFac;
	using Crm.Library.Rest;

	public class DynamicFormElementRestController : RestController<DynamicFormElement>
	{
		private readonly IDynamicFormElementTypeProvider dynamicFormElementProvider;
		private readonly IEnumerable<Lazy<Func<IDynamicFormElement>, DependencyMetadata>> dynamicFormElementFactories;

		public DynamicFormElementRestController(IDynamicFormElementTypeProvider dynamicFormElementProvider, RestTypeProvider restTypeProvider, IEnumerable<Lazy<Func<IDynamicFormElement>, DependencyMetadata>> dynamicFormElementFactories)
			: base(restTypeProvider)
		{
			this.dynamicFormElementProvider = dynamicFormElementProvider;
			this.dynamicFormElementFactories = dynamicFormElementFactories;
		}

		public virtual ActionResult ListElementTypes()
		{
			var elementTypes = dynamicFormElementProvider.ElementTypes.Values.Select(x => dynamicFormElementFactories.FirstOrDefault(y => y.Metadata.RegisteredType == x)?.Value() ?? Activator.CreateInstance(x)).ToList().Cast<DynamicFormElement>();
			return Rest(elementTypes.ToList(), "dynamicFormElementTypes");
		}
	}
}
