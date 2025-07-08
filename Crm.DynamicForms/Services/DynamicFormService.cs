using Crm.DynamicForms.Model;
using Crm.DynamicForms.Services.Interfaces;
using Crm.Library.Data.Domain.DataInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crm.DynamicForms.Services
{
	public class DynamicFormService : IDynamicFormService
	{
		private readonly IRepositoryWithTypedId<DynamicForm, Guid> dynamicFormRepository;
		private readonly IRepositoryWithTypedId<DynamicFormLanguage, Guid> dynamicFormLanguageRepository;

		public DynamicFormService(IRepositoryWithTypedId<DynamicForm, Guid> dynamicFormRepository,
			IRepositoryWithTypedId<DynamicFormLanguage, Guid> dynamicFormLanguageRepository)
		{
			this.dynamicFormRepository = dynamicFormRepository;
			this.dynamicFormLanguageRepository = dynamicFormLanguageRepository;
		}

		public virtual IEnumerable<string> GetUsedDynamicFormCategories()
		{
			return dynamicFormRepository.GetAll().Select(c => c.CategoryKey).Distinct();
		}

		public virtual IEnumerable<string> GetUsedLanguages()
		{
			return dynamicFormLanguageRepository.GetAll().Select(c => c.LanguageKey).Distinct();
		}
	}
}
