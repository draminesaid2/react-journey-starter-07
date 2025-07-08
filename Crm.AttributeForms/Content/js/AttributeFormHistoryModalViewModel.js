namespace("Crm.AttributeForms.ViewModels").AttributeFormHistoryModalViewModel = function(parentViewModel) {
	var viewModel = this;
	viewModel.loading = window.ko.observable(true);

	viewModel.parentViewModel = parentViewModel;
	viewModel.contactKey = window.ko.observable();
	viewModel.dynamicFormKey = window.ko.observable();
	viewModel.elementKey = window.ko.observable();
	viewModel.propertyName = window.ko.observable("");
	viewModel.users = window.ko.observableArray([]);

	window.Main.ViewModels.GenericListViewModel.call(viewModel,
		"CrmAttributeForms_AttributeForm",
		["CreateDate"],
		["DESC"],
		["DynamicForm.Languages"],
		false);

	viewModel.infiniteScroll(true);
};

namespace("Crm.AttributeForms.ViewModels").AttributeFormHistoryModalViewModel.prototype =
	Object.create(window.Main.ViewModels.GenericListViewModel.prototype);

namespace("Crm.AttributeForms.ViewModels").AttributeFormHistoryModalViewModel.prototype.init = function (id, params) {
	var viewModel = this;
	viewModel.contactKey(params.contactKey);
	viewModel.dynamicFormKey(params.dynamicFormKey);
	viewModel.elementKey(params.elementKey);
	viewModel.propertyName(params.propertyName);
	return window.Main.ViewModels.GenericListViewModel.prototype.init.apply(viewModel, arguments);
};

namespace("Crm.AttributeForms.ViewModels").AttributeFormHistoryModalViewModel.prototype.initItem = function(item) {
	return item;
};

namespace("Crm.AttributeForms.ViewModels").AttributeFormHistoryModalViewModel.prototype.initItems = function (attributeForms) {
	var viewModel = this;
	var attributeFormIds = attributeForms.map(function (x) { return x.Id; });
	return window.database.CrmDynamicForms_DynamicFormResponse
		.filter("it.DynamicFormElementKey === this.elementKey && it.DynamicFormReferenceKey in this.ids",
			{ elementKey: viewModel.elementKey(), ids: attributeFormIds })
		.orderByDescending(function (x) { return x.CreateDate; })
		.toArray()
		.then(function (responses) {
			var attributeForm = attributeForms[0];
			var element = window.ko.wrap.fromJS(viewModel.findElement(attributeForm));
			var result = [];
			responses.forEach(function (response, i) {
				var responseElement = window.ko.copy(element);
				var fakeViewModel = {
					DynamicFormElementRules: [],
					findResponseObject: function (formElement) { return response; }
				};
				window.Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.initFormElement.bind(fakeViewModel)(responseElement);
				responseElement.ResponseObject = window.ko.observable(response);
				var previousResponse = null;
				if (i === 0) {
					if (viewModel.items().length > 0) {
						previousResponse = viewModel.items()[viewModel.items().length - 1];
					}
				} else {
					if (result.length > 0) {
						previousResponse = result[result.length - 1];
					} else {
						previousResponse = viewModel.items()[viewModel.items().length - 1];
					}
				}
				if (!viewModel.isDifferentResponse(responseElement, previousResponse)) {
					previousResponse.ResponseObject(window.ko.unwrap(responseElement.ResponseObject));
					return;
				}
				result.push(responseElement);
			});
			var userIdsToLoad = window._.uniq(responses.map(x => x.ModifyUser))
				.filter(x => !viewModel.users().find(user => user.Id === x));
			if (userIdsToLoad.length === 0) {
				return result;
			}
			return window.database.Main_User
				.filter(function(it) { return it.Id in this.userIds; }, { userIds: userIdsToLoad }).forEach(
					function(user) {
						viewModel.users.push(user);
					}).then(function() {
					return result;
				});
		});
};

namespace("Crm.AttributeForms.ViewModels").AttributeFormHistoryModalViewModel.prototype.isDifferentResponse = function (responseElement, previousResponse) {
	if (previousResponse === null)
		return true;

	var formElementType = window.ko.unwrap(responseElement.FormElementType);

	var currentValue = responseElement.ResponseObject().Value;
	var previousValue = previousResponse.ResponseObject().Value;

	if (formElementType === "Date") {
		currentValue = new Date(responseElement.ResponseObject().Value);
		previousValue = new Date(previousResponse.ResponseObject().Value);
	} else if (formElementType === "CheckBoxList") {
		currentValue = JSON.stringify(JSON.parse(responseElement.ResponseObject().Value).sort());
		previousValue = JSON.stringify(JSON.parse(previousResponse.ResponseObject().Value).sort());
	}

	if ((currentValue || "").toString() === (previousValue || "").toString())
		return false;

	return true;
};

namespace("Crm.AttributeForms.ViewModels").AttributeFormHistoryModalViewModel.prototype.applyFilters = function (query) {
	var viewModel = this;
	return query.filter("it.ReferenceKey === this.referenceKey && it.DynamicFormKey === this.dynamicFormKey",
		{ referenceKey: viewModel.contactKey(), dynamicFormKey: viewModel.dynamicFormKey() });
};

namespace("Crm.AttributeForms.ViewModels").AttributeFormHistoryModalViewModel.prototype.applyJoins = function (query) {
	query = query.includeDynamicFormElements();
	return window.Main.ViewModels.GenericListViewModel.prototype.applyJoins.call(this, query);
};

namespace("Crm.AttributeForms.ViewModels").AttributeFormHistoryModalViewModel.prototype.findElement = function (attributeForm) {
	var viewModel = this;
	return attributeForm.DynamicForm.Elements.find(function (element) {
		return element.Id === this.elementKey;
	}, { elementKey: viewModel.elementKey() });
};

namespace("Crm.AttributeForms.ViewModels").AttributeFormHistoryModalViewModel.prototype.getLocalizationText = function (dynamicFormElement, choiceIndex, hint) {
	var viewModel = this;
	return window.Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.getLocalizationText.bind(viewModel.parentViewModel)(dynamicFormElement, choiceIndex, hint);
};