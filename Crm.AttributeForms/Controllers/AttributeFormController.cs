using Microsoft.AspNetCore.Mvc;

namespace Crm.AttributeForms.Controllers
{
	using Crm.AttributeForms.Model;
	using Crm.Library.Data.Domain.DataInterfaces;
	using System;
	using System.Linq;
	

	using Crm.DynamicForms.Services;
	using Crm.Library.Helper;
	using Crm.Library.Modularization;
	using Crm.ViewModels;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization.PermissionIntegration;

	using Microsoft.AspNetCore.Authorization;

	[Authorize]
	public class AttributeFormController : Controller
	{
		private readonly IRepositoryWithTypedId<AttributeForm, Guid> attributeFormRepository;
		private readonly IDynamicFormElementTypeProvider dynamicFormElementTypeProvider;
		public AttributeFormController(IRepositoryWithTypedId<AttributeForm, Guid> attributeFormRepository, IDynamicFormElementTypeProvider dynamicFormElementTypeProvider)
		{
			this.attributeFormRepository = attributeFormRepository;
			this.dynamicFormElementTypeProvider = dynamicFormElementTypeProvider;
		}

		[RenderAction("MaterialHeadResource", Priority = 1500)]
		[AllowAnonymous]
		public virtual ActionResult MaterialHeadResource() => Content(Url.JsResource("Crm.AttributeForms", "attributeFormMaterialJs") + Url.JsResource("Crm.AttributeForms", "attributeFormMaterialTs"));

		[RenderAction("ContactMaterialDetailsTabExtensions")]
		[RequiredPermission(PermissionName.Read, Group = nameof(AttributeForm))]
		public virtual ActionResult AttributeFormsExtensions() => PartialView();

		[RenderAction("MaterialGenericListResourceExtensions")]
		[RequiredPermission(PermissionName.Read, Group = nameof(AttributeForm))]
		public virtual ActionResult MaterialResponseTemplates()
		{
			var elementTypeNames = dynamicFormElementTypeProvider.ElementTypes.Keys.ToList();
			var model = new CrmModelList<string>
			{
				List = elementTypeNames
			};
			return PartialView(model);
		}

		[RequiredPermission(PermissionName.Read, Group = nameof(AttributeForm))]
		public virtual ActionResult MaterialFilterTemplates()
		{
			var elementTypeNames = dynamicFormElementTypeProvider.ElementTypes.Keys.ToList();
			var model = new CrmModelList<string>
			{
				List = elementTypeNames
			};
			return PartialView(model);
		}

		[RequiredPermission(PermissionName.Edit, Group = nameof(AttributeForm))]
		public virtual ActionResult MaterialEditTemplates()
		{
			var elementTypeNames = dynamicFormElementTypeProvider.ElementTypes.Keys.ToList();
			var model = new CrmModelList<string>
			{
				List = elementTypeNames
			};
			return PartialView(model);
		}

		[RequiredPermission(PermissionName.Create, Group = nameof(AttributeForm))]
		public virtual ActionResult CreateTemplate() => PartialView();

		[RequiredPermission(PermissionName.Read, Group = nameof(AttributeForm))]
		public virtual ActionResult HistoryTemplate() => PartialView();

		[RenderAction("ContactDetailsTopMenu")]
		[RequiredPermission(PermissionName.Create, Group = nameof(AttributeForm))]
		public virtual ActionResult ContactDetailsTopMenu() => PartialView();

		[RenderAction("DynamicFormDesignerAlert")]
		[RequiredPermission(PermissionName.Read, Group = nameof(AttributeForm))]
		public virtual ActionResult DynamicFormDesignerAlert() => PartialView();
		
		[RenderAction("FormDesignerHeaderExtensions")]
		[RequiredPermission(PermissionName.Read, Group = nameof(AttributeForm))]
		public virtual ActionResult FormDesignerHeaderExtension() => PartialView();

		[RenderAction("MaterialFormDesignerElementExtension")]
		[RequiredPermission(PermissionName.Read, Group = nameof(AttributeForm))]
		public virtual ActionResult MaterialFormDesignerElementFilterableExtension() => PartialView();

		[RenderAction("MaterialFormDesignerElementExtension")]
		[RequiredPermission(PermissionName.Read, Group = nameof(AttributeForm))]
		public virtual ActionResult MaterialFormDesignerElementDisplayExtension() => PartialView();

		[RenderAction("MaterialGenericListFilterExtensions")]
		[RequiredPermission(PermissionName.Read, Group = nameof(AttributeForm))]
		public virtual ActionResult MaterialGenericListFilterExtensions() => PartialView();

		[RenderAction("MaterialContactItemExtensions")]
		[RequiredPermission(PermissionName.Read, Group = nameof(AttributeForm))]
		public virtual ActionResult MaterialItemTemplateExtension() => PartialView();
	}
}
