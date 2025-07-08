using Microsoft.AspNetCore.Mvc;

namespace Crm.DynamicForms.Rest.Controller
{
	using System;
	using System.Linq;
	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Rest;

	public class DynamicFormRestController : RestController<DynamicForm>
	{
		private readonly IRepositoryWithTypedId<DynamicFormReference, Guid> dynamicFormReferenceRepository;
		private readonly IRepositoryWithTypedId<DynamicFormLanguage, Guid> dynamicFormLanguageRepository;
		private readonly IRepository<DynamicFormLocalization> dynamicFormLocalizationRepository;

		public DynamicFormRestController(IRepositoryWithTypedId<DynamicFormReference, Guid> dynamicFormReferenceRepository, IRepositoryWithTypedId<DynamicFormLanguage, Guid> dynamicFormLanguageRepository, RestTypeProvider restTypeProvider, IRepository<DynamicFormLocalization> dynamicFormLocalizationRepository)
			: base(restTypeProvider)
		{
			this.dynamicFormReferenceRepository = dynamicFormReferenceRepository;
			this.dynamicFormLanguageRepository = dynamicFormLanguageRepository;
			this.dynamicFormLocalizationRepository = dynamicFormLocalizationRepository;
		}

		public virtual ActionResult GetLanguages(Guid id)
		{
			var dynamicFormLanguages = dynamicFormLanguageRepository.GetAll().Where(x => x.DynamicFormKey == id);
			return Rest(dynamicFormLanguages.ToArray());
		}

		public virtual ActionResult DeleteLanguage(Guid id, string language)
		{
			var dynamicFormLocalizations = dynamicFormLocalizationRepository.GetAll().Where(x => x.DynamicFormId == id && x.Language == language);
			foreach (var dynamicFormLocalization in dynamicFormLocalizations)
			{
				dynamicFormLocalizationRepository.Delete(dynamicFormLocalization);
			}
			var dynamicFormLanguage = dynamicFormLanguageRepository.GetAll().FirstOrDefault(x => x.DynamicFormKey == id && x.LanguageKey == language);
			dynamicFormLanguageRepository.Delete(dynamicFormLanguage);
			return Rest(true);
		}

		public virtual ActionResult SaveLanguage(DynamicFormLanguage dynamicFormLanguage)
		{
			dynamicFormLanguageRepository.SaveOrUpdate(dynamicFormLanguage);
			return Rest(dynamicFormLanguage);
		}

		public virtual ActionResult SaveLanguageFileResource(DynamicFormLanguage dynamicFormLanguage)
		{
			DynamicFormLanguage entity = dynamicFormLanguageRepository.Get(dynamicFormLanguage.Id);
			entity.FileResourceId = dynamicFormLanguage.FileResourceId;
			return Rest(entity);
		}

		public virtual ActionResult SaveFormReference(DynamicFormReference restFormReference)
		{
			var formReference = dynamicFormReferenceRepository.Get(restFormReference.Id);
			formReference.Completed = restFormReference.Completed;

			foreach (var newResponse in restFormReference.Responses)
			{
				var response = formReference.Responses.FirstOrDefault(x => x.Equals(newResponse));
				if (response == null)
				{
					formReference.Responses.Add(newResponse);
				}
				else
				{
					response.ValueAsString = newResponse.ValueAsString;
				}
			}

			dynamicFormReferenceRepository.SaveOrUpdate(formReference);
			return Rest(new { formReference.Id });
		}
	}
}
