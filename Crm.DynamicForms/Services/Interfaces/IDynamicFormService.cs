using Crm.Library.AutoFac;
using System.Collections.Generic;

namespace Crm.DynamicForms.Services.Interfaces
{
	public interface IDynamicFormService : IDependency
	{
		IEnumerable<string> GetUsedDynamicFormCategories();
		IEnumerable<string> GetUsedLanguages();
	}
}
