using Crm.DynamicForms.Model.Lookups;
using Crm.DynamicForms.Services.Interfaces;
using Crm.Library.Globalization;
using Crm.Library.Globalization.Lookup;
using System;
using System.Collections.Generic;

namespace Crm.DynamicForms
{
	public class DynamicFormsUsedLookupsProvider : IUsedLookupsProvider
	{
		private readonly IDynamicFormService dynamicFormService;
		public DynamicFormsUsedLookupsProvider(IDynamicFormService dynamicFormService)
		{
			this.dynamicFormService = dynamicFormService;
		}

		public virtual IEnumerable<object> GetUsedLookupKeys(Type lookupType)
		{
			if (lookupType == typeof(DynamicFormCategory))
			{
				return dynamicFormService.GetUsedDynamicFormCategories();
			}

			if (lookupType == typeof(Language))
			{
				return dynamicFormService.GetUsedLanguages();
			}

			return new List<object>();
		}
	}
}
