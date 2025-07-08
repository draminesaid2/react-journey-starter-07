namespace Crm.DynamicForms.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Text;
	using AutoMapper;
	using Crm.Controllers;
	using Crm.DynamicForms.Model;
	using Crm.DynamicForms.Model.BaseModel;
	using Crm.DynamicForms.Model.Extensions;
	using Crm.DynamicForms.Model.Lookups;
	using Crm.DynamicForms.Rest.Model;
	using Crm.DynamicForms.Services;
	using Crm.DynamicForms.ViewModels;
	using Crm.Library.AutoFac;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Modularization;
	using Crm.Library.Rest;
	using Crm.Library.Services.Interfaces;
	using Crm.Model;
	using Crm.Rest.Model;
	using Crm.Results;
	using Crm.Services;
	using Crm.Services.Interfaces;
	using Crm.ViewModels;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	public class DynamicFormController : CrmController
	{
		private readonly IRepositoryWithTypedId<DynamicForm, Guid> dynamicFormRepository;
		private readonly IRepositoryWithTypedId<FileResource, Guid> fileResourceRepository;
		private readonly IRepositoryWithTypedId<DynamicFormReference, Guid> dynamicFormReferenceRepository;
		private readonly IEnumerable<Lazy<Func<DynamicFormReference, IResponseViewModel>, DependencyMetadata>> responseViewModelFactories;
		private readonly IPdfService pdfService;
		private readonly IDynamicFormElementTypeProvider dynamicFormElementTypeProvider;
		private readonly IUserService userService;
		private readonly IRepositoryWithTypedId<DynamicFormLanguage, Guid> dynamicFormLanguageRepository;
		private readonly IRestSerializer restSerializer;
		private readonly RestTypeProvider restTypeProvider;
		private readonly IRepositoryWithTypedId<DynamicFormElement, Guid> dynamicFormElementRepository;
		private readonly IRepositoryWithTypedId<DynamicFormElementRule, Guid> dynamicFormElementRuleRepository;
		private readonly IRepositoryWithTypedId<DynamicFormElementRuleCondition, Guid> dynamicFormElementRuleConditionRepository;
		private readonly IRepository<DynamicFormLocalization> dynamicFormLocalizationRepository;
		private readonly IRepositoryWithTypedId<DynamicFormResponse, Guid> dynamicFormResponseRepository;
		private readonly IMapper mapper;
		private readonly Func<DynamicFormResponse> dynamicFormResponseFactory;
		private readonly Func<DynamicFormReference> dynamicFormReferenceFactory;
		private readonly Func<DynamicFormLanguage> dynamicFormLanguageFactory;
		private readonly Func<DynamicFormLocalization> dynamicFormLocalizationFactory;
		private readonly IFileService fileService;
		private readonly IRuleValidationService ruleValidationService;
		private readonly IRepositoryWithTypedId<DynamicFormFileResponse, Guid> dynamicFormFileResponseRepository;
		private readonly ILookupManager lookupManager;
		private readonly IResourceManager resourceManager;

		public DynamicFormController(IPdfService pdfService, RestTypeProvider restTypeProvider, IRenderViewToStringService renderViewToStringService, ILookupManager lookupManager, IResourceManager resourceManager, IRuleValidationService ruleValidationService, IRepositoryWithTypedId<DynamicForm, Guid> dynamicFormRepository, IRepositoryWithTypedId<DynamicFormReference, Guid> dynamicFormReferenceRepository, IEnumerable<Lazy<Func<DynamicFormReference, IResponseViewModel>, DependencyMetadata>> responseViewModelFactories, IDynamicFormElementTypeProvider dynamicFormElementTypeProvider, IUserService userService, IRepositoryWithTypedId<DynamicFormLanguage, Guid> dynamicFormLanguageRepository, IRestSerializer restSerializer, IRepositoryWithTypedId<DynamicFormElement, Guid> dynamicFormElementRepository, IMapper mapper, Func<DynamicFormResponse> dynamicFormResponseFactory, Func<DynamicFormReference> dynamicFormReferenceFactory, Func<DynamicFormLanguage> dynamicFormLanguageFactory, Func<DynamicFormLocalization> dynamicFormLocalizationFactory, IRepositoryWithTypedId<DynamicFormElementRule, Guid> dynamicFormElementRuleRepository, IRepositoryWithTypedId<DynamicFormElementRuleCondition, Guid> dynamicFormElementRuleConditionRepository, IRepository<DynamicFormLocalization> dynamicFormLocalizationRepository, IRepositoryWithTypedId<DynamicFormResponse, Guid> dynamicFormResponseRepository, IRepositoryWithTypedId<FileResource, Guid> fileResourceRepository, IRepositoryWithTypedId<DynamicFormFileResponse, Guid> dynamicFormFileResponseRepository, IFileService fileService)
			: base(pdfService, renderViewToStringService)
		{
			this.dynamicFormRepository = dynamicFormRepository;
			this.dynamicFormReferenceRepository = dynamicFormReferenceRepository;
			this.responseViewModelFactories = responseViewModelFactories;
			this.pdfService = pdfService;
			this.restTypeProvider = restTypeProvider;
			this.lookupManager = lookupManager;
			this.resourceManager = resourceManager;
			this.ruleValidationService = ruleValidationService;
			this.dynamicFormElementTypeProvider = dynamicFormElementTypeProvider;
			this.userService = userService;
			this.dynamicFormLanguageRepository = dynamicFormLanguageRepository;
			this.restSerializer = restSerializer;
			this.dynamicFormElementRepository = dynamicFormElementRepository;
			this.mapper = mapper;
			this.dynamicFormResponseFactory = dynamicFormResponseFactory;
			this.dynamicFormReferenceFactory = dynamicFormReferenceFactory;
			this.dynamicFormLanguageFactory = dynamicFormLanguageFactory;
			this.dynamicFormLocalizationFactory = dynamicFormLocalizationFactory;
			this.dynamicFormElementRuleRepository = dynamicFormElementRuleRepository;
			this.dynamicFormElementRuleConditionRepository = dynamicFormElementRuleConditionRepository;
			this.dynamicFormLocalizationRepository = dynamicFormLocalizationRepository;
			this.dynamicFormResponseRepository = dynamicFormResponseRepository;
			this.dynamicFormFileResponseRepository = dynamicFormFileResponseRepository;
			this.fileService = fileService;
			this.fileResourceRepository = fileResourceRepository;
		}
		public virtual ActionResult DisplayTemplates()
		{
			var elementTypeNames = dynamicFormElementTypeProvider.ElementTypes.Keys.ToList();
			var model = new CrmModelList<string> { List = elementTypeNames };
			return PartialView(model);
		}

		[RenderAction("DynamicFormItemTemplateActions", Priority = 100)]
		[RequiredPermission(PermissionName.Edit, Group = DynamicFormsPlugin.PermissionGroup.DynamicForms)]
		public virtual ActionResult ItemTemplateActionEdit()
		{
			return PartialView();
		}

		[RenderAction("DynamicFormItemTemplateActions", Priority = 90)]
		[RequiredPermission(PermissionName.Delete, Group = DynamicFormsPlugin.PermissionGroup.DynamicForms)]
		public virtual ActionResult ItemTemplateActionDelete()
		{
			return PartialView();
		}

		public virtual ActionResult EditorTemplates()
		{
			var elementTypeNames = dynamicFormElementTypeProvider.ElementTypes.Keys.ToList();
			var model = new CrmModelList<string> { List = elementTypeNames };
			return PartialView(model);
		}

		public virtual ActionResult MaterialEditorTemplates()
		{
			var elementTypeNames = dynamicFormElementTypeProvider.ElementTypes.Keys.ToList();
			var model = new CrmModelList<string> { List = elementTypeNames };
			return PartialView(model);
		}

		[RenderAction("FormDesignerSidebarTabContent", Priority = 100)]
		public virtual ActionResult FormDesignerSidebarTabContentAddFields()
		{
			return PartialView();
		}

		[RenderAction("FormDesignerSidebarTabHeader", Priority = 100)]
		public virtual ActionResult FormDesignerSidebarTabHeaderAddFields()
		{
			return PartialView();
		}

		[RenderAction("FormDesignerSidebarTabContent", Priority = 90)]
		public virtual ActionResult FormDesignerSidebarTabContentEditField()
		{
			return PartialView();
		}

		[RenderAction("FormDesignerSidebarTabHeader", Priority = 90)]
		public virtual ActionResult FormDesignerSidebarTabHeaderEditField()
		{
			return PartialView();
		}

		[RenderAction("FormDesignerSidebarTabContent", Priority = 80)]
		public virtual ActionResult FormDesignerSidebarTabContentEditForm()
		{
			return PartialView();
		}

		[RenderAction("FormDesignerSidebarTabHeader", Priority = 80)]
		public virtual ActionResult FormDesignerSidebarTabHeaderEditForm()
		{
			return PartialView();
		}

		public virtual ActionResult FormElementRuleEditor()
		{
			return PartialView();
		}

		[RenderAction("MaterialHeadResource", Priority = 6990)]
		public virtual ActionResult HeadResource()
		{
			return Content(Url.JsResource("Crm.DynamicForms", "dynamicFormsMaterialJs") + Url.JsResource("Crm.DynamicForms", "dynamicFormsMaterialTs"));
		}

		[RenderAction("MaterialTitleResource", Priority = 6990)]
		public virtual ActionResult TitleResource()
		{
			return Content(Url.CssResource("Crm.DynamicForms", "dynamicFormsMaterialCss"));
		}

		[HttpPost]
		public virtual ActionResult Import(IFormFile jsonFile)
		{
			DynamicFormExport dynamicFormExport;
			using (var memoryStream = new MemoryStream())
			{
				jsonFile.OpenReadStream().CopyTo(memoryStream);
				byte[] data = memoryStream.ToArray();
				var json = Encoding.UTF8.GetString(data);
				try
				{
					dynamicFormExport = (DynamicFormExport)restSerializer.Parse(json, typeof(DynamicFormExport));
				}
				catch
				{
					return new JsonResult(new { Guid.Empty });
				}
			}

			var dynamicForm = mapper.Map<DynamicForm>(dynamicFormExport.DynamicForm);
			dynamicForm.Id = Guid.Empty;
			dynamicForm.AuthData = null;
			dynamicFormRepository.SaveOrUpdate(dynamicForm);
			foreach (var dynamicFormLanguageRest in dynamicFormExport.Languages)
			{
				var dynamicFormLanguage = mapper.Map<DynamicFormLanguage>(dynamicFormLanguageRest);
				dynamicFormLanguage.DynamicFormKey = dynamicForm.Id;
				dynamicFormLanguage.Id = Guid.Empty;
				dynamicFormLanguage.AuthData = null;
				dynamicFormLanguage.StatusKey = DynamicFormStatus.DraftKey;
				dynamicFormLanguageRepository.SaveOrUpdate(dynamicFormLanguage);
			}

			var fileIds = new List<Guid>();
			foreach (var file in dynamicFormExport.ImageContents)
			{
				var deserializedFile = (FileResourceRest)restSerializer.Parse(file, typeof(FileResourceRest));
				var fileResource = mapper.Map<FileResource>(deserializedFile);
				fileResource.Id = Guid.Empty;
				fileResource.AuthData = null;
				var savedFile = fileResourceRepository.SaveOrUpdate(fileResource);
				fileIds.Add(savedFile.Id);
			}

			var dynamicFormElementIds = new Dictionary<Guid, Guid>();
			int counter = 0;
			foreach (var dynamicFormElementRest in dynamicFormExport.Elements)
			{
				var dynamicFormElementRestType = dynamicFormElementRest.GetType();
				var dynamicFormElementType = restTypeProvider.GetDomainType(dynamicFormElementRestType);
				var dynamicFormElement = (DynamicFormElement)mapper.Map(dynamicFormElementRest, dynamicFormElementRestType, dynamicFormElementType);
				var oldDynamicFormElementId = dynamicFormElement.Id;
				dynamicFormElement.DynamicFormKey = dynamicForm.Id;
				dynamicFormElement.Id = Guid.Empty;
				dynamicFormElement.AuthData = null;
				if (dynamicFormElement.FormElementType == Image.DiscriminatorValue)
				{
					var imgElem = dynamicFormElement as Image;
					imgElem.FileResourceId = fileIds[counter++];
				}

				dynamicFormElementRepository.SaveOrUpdate(dynamicFormElement);
				dynamicFormElementIds[oldDynamicFormElementId] = dynamicFormElement.Id;
			}

			foreach (var dynamicFormLocalizationRest in dynamicFormExport.Localizations)
			{
				if (dynamicFormLocalizationRest.Value == string.Empty)
					continue;
				var dynamicFormLocalization = mapper.Map<DynamicFormLocalization>(dynamicFormLocalizationRest);
				dynamicFormLocalization.DynamicFormId = dynamicForm.Id;
				dynamicFormLocalization.DynamicFormElementId = dynamicFormLocalization.DynamicFormElementId.HasValue ? dynamicFormElementIds[dynamicFormLocalization.DynamicFormElementId.Value] : (Guid?)null;
				dynamicFormLocalization.Id = 0;
				dynamicFormLocalizationRepository.SaveOrUpdate(dynamicFormLocalization);
			}

			foreach (var dynamicFormElementRuleRest in dynamicFormExport.Elements.SelectMany(x => x.Rules))
			{
				var dynamicFormElementRule = mapper.Map<DynamicFormElementRule>(dynamicFormElementRuleRest);
				dynamicFormElementRule.DynamicFormId = dynamicForm.Id;
				dynamicFormElementRule.DynamicFormElementId = dynamicFormElementIds[dynamicFormElementRule.DynamicFormElementId];
				dynamicFormElementRule.Id = Guid.Empty;
				dynamicFormElementRuleRepository.SaveOrUpdate(dynamicFormElementRule);
				foreach (var dynamicFormElementRuleCondition in dynamicFormElementRule.Conditions)
				{
					dynamicFormElementRuleCondition.DynamicFormElementId = dynamicFormElementIds[dynamicFormElementRuleCondition.DynamicFormElementId];
					dynamicFormElementRuleCondition.DynamicFormElementRuleId = dynamicFormElementRule.Id;
					dynamicFormElementRuleCondition.Id = Guid.Empty;
					dynamicFormElementRuleConditionRepository.SaveOrUpdate(dynamicFormElementRuleCondition);
				}
			}

			return new JsonResult(new { dynamicForm.Id });
		}

		public virtual ActionResult LocalizationEditor(bool multiline = false)
		{
			var model = new CrmModelItem<bool> { Item = multiline };
			return PartialView(model);
		}

		public virtual ActionResult MaterialDisplayTemplates()
		{
			var elementTypeNames = dynamicFormElementTypeProvider.ElementTypes.Keys.ToList();
			var model = new CrmModelList<string> { List = elementTypeNames };
			return PartialView(model);
		}

		[AllowAnonymous]
		public virtual ActionResult ResponseTemplates()
		{
			var elementTypeNames = dynamicFormElementTypeProvider.ElementTypes.Keys.ToList();
			var model = new CrmModelList<string> { List = elementTypeNames };
			return PartialView(model);
		}

		[HttpGet]
		[RequiredPermission(PermissionName.Create, Group = DynamicFormsPlugin.PermissionGroup.DynamicForms)]
		public virtual ActionResult CreateTemplate()
		{
			return PartialView();
		}

		[HttpGet]
		[RequiredPermission(PermissionName.Edit, Group = DynamicFormsPlugin.PermissionGroup.DynamicForms)]
		public virtual ActionResult EditTemplate()
		{
			return PartialView();
		}

		[HttpPost]
		[RequiredPermission(DynamicFormsPlugin.PermissionGroup.DynamicForms, false, PermissionName.Create, PermissionName.Edit)]
		public virtual ActionResult Save(DynamicForm form, string title, string description)
		{
			var model = new CrmModelItem<DynamicForm> { Item = form };
			if (!form.Localizations.Any())
			{
				var formLocalization = dynamicFormLocalizationFactory();
				formLocalization.Language = userService.CurrentUser.DefaultLanguageKey;
				form.Localizations.Add(formLocalization);
				form.DefaultLanguageKey = userService.CurrentUser.DefaultLanguageKey;
			}

			var dynamicFormLocalization = form.Localizations.First();
			dynamicFormLocalization.Value = title;
			dynamicFormLocalization.Hint = description;
			var ruleViolations = ruleValidationService.GetRuleViolations(form, dynamicFormLocalization);
			if (ruleViolations.Any())
			{
				model.AddRuleViolations(ruleViolations);
				var viewName = form.IsTransient() ? "Create" : "Edit";
				return View(viewName, model);
			}

			dynamicFormRepository.SaveOrUpdate(form);

			if (form.Languages.Count == 0)
			{
				var dynamicFormLanguage = dynamicFormLanguageFactory();
				dynamicFormLanguage.DynamicForm = form;
				dynamicFormLanguage.DynamicFormKey = form.Id;
				dynamicFormLanguage.LanguageKey = userService.CurrentUser.DefaultLanguageKey;
				dynamicFormLanguage.StatusKey = lookupManager.GetDefault<DynamicFormStatus>();
				dynamicFormLanguageRepository.SaveOrUpdate(dynamicFormLanguage);
				dynamicFormLocalization.DynamicFormId = form.Id;
				dynamicFormLocalizationRepository.SaveOrUpdate(dynamicFormLocalization);
			}

			return Json(new { success = true, action = "Edit", id = form.Id });
		}
		public virtual ActionResult Export(Guid id)
		{
			var form = dynamicFormRepository.Get(id);
			var imageContents = new List<string>();
			foreach (var element in form.Elements)
			{
				if (element.FormElementType == Image.DiscriminatorValue)
				{
					var fileResource = element as Image;
					if (fileResource.FileResourceId.HasValue)
					{
						imageContents.Add(restSerializer.SerializeAsJson(mapper.Map<FileResourceRest>(fileResourceRepository.Get(fileResource.FileResourceId.Value))));
					}
				}
			}

			var export = mapper.Map<DynamicFormExport>(form);
			export.ImageContents = imageContents.ToArray();
			var json = restSerializer.SerializeAsJson(export);
			var bytes = Encoding.UTF8.GetBytes(json);
			var dynamicFormTitle = form.GetTitle() ?? resourceManager.GetTranslation("DynamicForm");
			return File(bytes, "application/json", $"{dynamicFormTitle.Replace(Path.GetInvalidFileNameChars(), '_')}_{form.Id}_{form.ModifyDate.ToString("s", CultureInfo.InvariantCulture)}.json");
		}

		public virtual ActionResult CheckDynamicFormUsage(Guid dynamicFormId)
		{
			var isUsed = dynamicFormReferenceRepository.GetAll().Any(x => x.DynamicFormKey == dynamicFormId);
			return Json(new { isUsed });
		}

		public virtual ActionResult CheckDynamicFormElementUsage(Guid dynamicFormElementId)
		{
			var hasResponse = dynamicFormResponseRepository.GetAll().Any(x => x.DynamicFormElementKey == dynamicFormElementId);
			return Json(new { hasResponse });
		}

		[HttpPost]
		[RequiredPermission(PermissionName.Delete, Group = DynamicFormsPlugin.PermissionGroup.DynamicForms)]
		public virtual ActionResult Delete(Guid dynamicFormId)
		{
			var form = dynamicFormRepository.Get(dynamicFormId);
			var isUsed = dynamicFormReferenceRepository.GetAll().Any(x => x.DynamicFormKey == dynamicFormId);
			if (isUsed)
			{
				foreach (var dynamicFormLanguage in form.Languages)
				{
					dynamicFormLanguage.StatusKey = DynamicFormStatus.DisabledKey;
				}

				return new EmptyResult();
			}

			foreach (var dynamicFormLanguage in form.Languages)
			{
				if (dynamicFormLanguage.FileResourceId != null)
				{
					var fileResource = fileResourceRepository.Get((Guid)dynamicFormLanguage.FileResourceId);
					fileResourceRepository.Delete(fileResource);
				}
			}
			dynamicFormRepository.Delete(form);
			return new EmptyResult();
		}

		public virtual ActionResult DynamicFormPageHeader(Guid? id)
		{
			if (id.IsNotNullOrDefault())
			{
				var dynamicFormReference = dynamicFormReferenceRepository.Get(id.Value);
				if (dynamicFormReference == null)
				{
					return new ErrorResult(ErrorViewModel.NotFound);
				}

				var model = GetModel(dynamicFormReference);
				return View("DynamicFormPageHeader", model);
			}

			return View("DynamicFormPageHeader");
		}
		public virtual ActionResult DynamicFormPageFooter(Guid? id)
		{
			if (id.IsNotNullOrDefault())
			{
				var dynamicFormReference = dynamicFormReferenceRepository.Get(id.Value);
				if (dynamicFormReference == null)
				{
					return new ErrorResult(ErrorViewModel.NotFound);
				}

				var model = GetModel(dynamicFormReference);
				return View("DynamicFormPageFooter", model);
			}

			return View("DynamicFormPageFooter");
		}

		public virtual ActionResult DynamicFormResponse(Guid? id, string output = "PDF")
		{
			if (id.IsNotNullOrDefault())
			{
				var dynamicFormReference = dynamicFormReferenceRepository.Get(id.Value);
				if (dynamicFormReference == null)
				{
					return new ErrorResult(ErrorViewModel.NotFound);
				}

				var model = GetModel(dynamicFormReference);
				if (dynamicFormReference.DynamicForm.CategoryKey == "PDF-Checklist")
				{
					var fileResponses = dynamicFormFileResponseRepository.GetAll().Where(x => x.DynamicFormReferenceKey == id).ToArray();
					Guid fileResourceId = Guid.Empty;
					foreach (DynamicFormFileResponse dynamicFormFileResponse in fileResponses)
					{
						if (dynamicFormFileResponse.Language.Key == userService.CurrentUser.DefaultLanguageKey)
						{
							fileResourceId = (Guid)dynamicFormFileResponse.FileResourceId;
						}
					}

					if (fileResourceId == Guid.Empty)
					{
						var dynamicFormDefaultLanguageKey = dynamicFormRepository.Get(dynamicFormReference.DynamicFormKey).DefaultLanguageKey;
						fileResourceId = (Guid)dynamicFormFileResponseRepository.GetAll().Where(x => x.DynamicFormReferenceKey == id && x.LanguageKey == dynamicFormDefaultLanguageKey).First().FileResourceId;
					}

					var fileResource = fileService.GetFile(fileResourceId);
					return Pdf(fileResource.Content, fileResource.Filename);
				}

				if (output.Equals("PDF", StringComparison.InvariantCultureIgnoreCase))
				{
					var filename = dynamicFormReference.GetFileName(resourceManager);
					var reportHtml = renderViewToStringService.RenderViewToString(DynamicFormsPlugin.PluginName, "DynamicForm", "Response", model);
					var reportHeaderHtml = renderViewToStringService.RenderViewToString(DynamicFormsPlugin.PluginName, "DynamicForm", "DynamicFormPageHeader", model);
					var reportFooterHtml = renderViewToStringService.RenderViewToString(DynamicFormsPlugin.PluginName, "DynamicForm", "DynamicFormPageFooter", model);
					var reportPdf = pdfService.Html2Pdf(reportHtml, reportHeaderHtml, model.HeaderContentSize, model.HeaderContentSpacing, reportFooterHtml, model.FooterContentSize, model.FooterContentSpacing);
					return Pdf(reportPdf, filename);
				}

				if (output.Equals("HTML", StringComparison.InvariantCultureIgnoreCase))
				{
					return View("Response", model);
				}

				throw new ArgumentException(String.Format("Invalid output type ({0}), use PDF or HTML instead", output));
			}

			return View("Response");
		}

		public virtual ActionResult DynamicFormResponsePreview(Guid? id, string output = "PDF")
		{
			if (id.IsNotNullOrDefault())
			{
				var dynamicForm = dynamicFormRepository.Get(id.Value);
				if (dynamicForm == null)
				{
					return new ErrorResult(ErrorViewModel.NotFound);
				}

				var responses = new List<DynamicFormResponse>();
				foreach (var dynamicFormElement in dynamicForm.Elements)
				{
					var dynamicFormResponse = dynamicFormResponseFactory();
					dynamicFormResponse.DynamicFormElement = dynamicFormElement;
					dynamicFormResponse.DynamicFormElementKey = dynamicFormElement.Id;
					dynamicFormResponse.DynamicFormElementType = dynamicFormElement.FormElementType;
					responses.Add(dynamicFormResponse);
				}

				dynamicForm.HideEmptyOptional = false;
				var dynamicFormReference = dynamicFormReferenceFactory();
				dynamicFormReference.DynamicForm = dynamicForm;
				dynamicFormReference.Responses = responses;
				var model = GetModel(dynamicFormReference);
				if (output.Equals("PDF", StringComparison.InvariantCultureIgnoreCase))
				{
					var filename = $"{dynamicForm.GetTitle() ?? resourceManager.GetTranslation("DynamicForm")}-{id}";
					var reportHtml = renderViewToStringService.RenderViewToString(DynamicFormsPlugin.PluginName, "DynamicForm", "Response", model);
					var reportHeaderHtml = renderViewToStringService.RenderViewToString(DynamicFormsPlugin.PluginName, "DynamicForm", "DynamicFormPageHeader", model);
					var reportFooterHtml = renderViewToStringService.RenderViewToString(DynamicFormsPlugin.PluginName, "DynamicForm", "DynamicFormPageFooter", model);
					var reportPdf = pdfService.Html2Pdf(reportHtml, reportHeaderHtml, model.HeaderContentSize, model.HeaderContentSpacing, reportFooterHtml, model.FooterContentSize, model.FooterContentSpacing);
					return Pdf(reportPdf, filename);
				}

				if (output.Equals("HTML", StringComparison.InvariantCultureIgnoreCase))
				{
					return View("Response", model);
				}

				throw new ArgumentException(String.Format("Invalid output type ({0}), use PDF or HTML instead", output));
			}

			return View("Response");
		}

		protected virtual IResponseViewModel GetModel(DynamicFormReference dynamicFormReference)
		{
			var targetType = typeof(IResponseViewModel<>).MakeGenericType(dynamicFormReference.GetType());
			var responseViewModelFactory = responseViewModelFactories.FirstOrDefault(x => targetType.IsAssignableFrom(x.Metadata.RegisteredType))?.Value ?? responseViewModelFactories.First().Value;
			var model = responseViewModelFactory(dynamicFormReference);
			return model;
		}

		[RenderAction("TemplateHeadResource", Priority = 9990)]
		[AllowAnonymous]
		public virtual ActionResult DynamicFormsResponseJs() => Content(Url.JsResource("Crm.DynamicForms", "dynamicFormsResponseJs"));

		[RenderAction("TemplateHeadResource", Priority = 9970)]
		[AllowAnonymous]
		public virtual ActionResult TemplateHeadResourceDynamicFormsCss() => Content(Url.CssResource("Crm.DynamicForms", "dynamicFormsCss"));
	}
}
