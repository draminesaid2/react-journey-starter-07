namespace("Crm.AttributeForms.ViewModels").AttributeFormCreateModalViewModel = function(parentViewModel) {
	var viewModel = this;
	viewModel.loading = window.ko.observable(true);

	viewModel.currentUser = window.ko.observable(null);
	viewModel.contactId = window.ko.observable();
	viewModel.contactType = window.ko.observable();
	viewModel.attributeForm = window.ko.observable(null);
	viewModel.attachedDynamicFormIds = window.ko.observableArray([]);

	viewModel.errors = window.ko.validation.group(viewModel.attributeForm, { deep: true });
};
namespace("Crm.AttributeForms.ViewModels").AttributeFormCreateModalViewModel.prototype.init = function (id, params) {
	var viewModel = this;
	viewModel.contactId(params.contactId);
	viewModel.contactType(params.contactType);
	return window.Helper.User.getCurrentUser().then(function (user) {
		viewModel.currentUser(user);
		var newAttributeForm = window.database.CrmAttributeForms_AttributeForm
			.CrmAttributeForms_AttributeForm.create();
		newAttributeForm.ReferenceKey = viewModel.contactId();
		window.database.add(newAttributeForm);
		return newAttributeForm;
	}).then(function (attributeForm) {
		viewModel.attributeForm(attributeForm.asKoObservable());
	}).then(function() {
		window.database.CrmAttributeForms_AttributeForm
			.filter(function (it) {
				return it.ReferenceKey === this.referenceKey && !it.Completed;
			}, { referenceKey: viewModel.contactId() })
			.map(function(attributeForm) {
				return attributeForm.DynamicFormKey;
			})
			.toArray(viewModel.attachedDynamicFormIds);
	});
};
namespace("Crm.AttributeForms.ViewModels").AttributeFormCreateModalViewModel.prototype.dispose = function () {
	var viewModel = this;
	window.database.detach(viewModel.attributeForm().innerInstance);
};
namespace("Crm.AttributeForms.ViewModels").AttributeFormCreateModalViewModel.prototype.save = function() {
	var viewModel = this;
	viewModel.loading(true);

	if (viewModel.errors().length > 0) {
		viewModel.loading(false);
		viewModel.errors.showAllMessages();
		return;
	}

	if (viewModel.attachedDynamicFormIds().length + 1 > window.Crm.AttributeForms.Settings.AttributeFormsCountPerContact) {
		viewModel.loading(false);
		$(".modal:visible").modal("hide");
		window.swal(window.Helper.String.getTranslatedString("Error"),
			window.Helper.String.getTranslatedString("MaxAttributeFormsCountReached").replace("{0}", window.Crm.AttributeForms.Settings.AttributeFormsCountPerContact),
			"error");
		return;
	}

	window.database.saveChanges().then(function() {
		viewModel.loading(false);
		$(".modal:visible").modal("hide");
	}).fail(function() {
		viewModel.loading(false);
		window.swal(window.Helper.String.getTranslatedString("UnknownError"),
			window.Helper.String.getTranslatedString("Error_InternalServerError"),
			"error");
	});
};
namespace("Crm.AttributeForms.ViewModels").AttributeFormCreateModalViewModel.prototype.getDynamicFormAutocompleteFilter = function (query, value) {
	var viewModel = this;
	return query
		.filter("!(it.Id in ids)", { ids: viewModel.attachedDynamicFormIds() })
		.filter("it.CategoryKey === 'AttributeForm'")
		.filter("it.ExtensionValues.ContactType === contactType", { contactType: viewModel.contactType() })
		.filter("filterByDynamicFormTitle", { filter: (value || ""), languageKey: this.currentUser().DefaultLanguageKey, statusKey: 'Released'});
};
namespace("Crm.AttributeForms.ViewModels").AttributeFormCreateModalViewModel.prototype.getDynamicFormAutocompleteDisplay = function (dynamicForm) {
	var viewModel = this;
	var currentUserDefaultLanguage = viewModel.currentUser().DefaultLanguageKey;
	var currentUserDefaultLanguageReleased = dynamicForm.Languages.some(function (x) {
		return x.LanguageKey === currentUserDefaultLanguage && x.StatusKey === "Released";
	});
	var localization = null;
	if (currentUserDefaultLanguageReleased) {
		localization = window.ko.utils.arrayFirst(dynamicForm.Localizations,
			function (x) {
				return x.Language === currentUserDefaultLanguage;
			});
	}
	if (!localization) {
		localization = window.ko.utils.arrayFirst(dynamicForm.Localizations,
			function (x) {
				return x.Language === dynamicForm.DefaultLanguageKey;
			});
	}
	if (localization) {
		return localization.Value;
	}
	return null;
};
namespace("Crm.AttributeForms.ViewModels").AttributeFormCreateModalViewModel.prototype.onDynamicFormSelect = function (dynamicForm) {
	var viewModel = this;
	if (dynamicForm) {
		viewModel.attributeForm().DynamicFormKey(dynamicForm);
	} else {
		viewModel.attributeForm().DynamicFormKey(null);
	}
};