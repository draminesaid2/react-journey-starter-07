(function () {
	var baseInit = window.Main.ViewModels.ContactListViewModel.prototype.init;
	window.Main.ViewModels.ContactListViewModel.prototype.init = function () {
		const viewModel = this;
		const args = arguments;
		viewModel.attributeFormFilters = window.ko.observableArray([]);
		viewModel.localizations = window.ko.observableArray([]);
		viewModel.dynamicFormsLocalizationQueries = [];
		viewModel.localizationQueries = [];
		return window.Helper.Culture.languageCulture()
			.then(function(language) {
				let contactType = viewModel.entityType.substr(viewModel.entityType.indexOf('_') + 1);
				if(contactType === "ServiceOrderHead"){
					contactType = "ServiceOrder";
				}
				return window.database.CrmDynamicForms_DynamicForm
					.includeElements()
					.include("Languages")
					.filter("it.CategoryKey === 'AttributeForm'")
					.filter("it.ExtensionValues.ContactType === this.contactType", { contactType: contactType })
					.toArray(function(dynamicForms) {
						dynamicForms.forEach(function(dynamicForm) {
							if (!dynamicForm.Languages.some(language => language.StatusKey === 'Released'))
								return;
							dynamicForm.Elements = dynamicForm.Elements.sort(function(a, b) { return a.SortOrder - b.SortOrder; });
							dynamicForm.Elements.forEach(function(dynamicFormElement) {
								if (!dynamicFormElement.ExtensionValues.Filterable)
									return;
								viewModel.addDynamicFormElementLocalizationQuery(dynamicForm, dynamicFormElement, language);
								dynamicFormElement.Response = null;
								viewModel.attributeFormFilters.push(window.ko.wrap.fromJS(dynamicFormElement));
							});
						});
					});
			}).then(function() {
				return baseInit.apply(viewModel, args);
			});
	};

	const baseInitItems = window.Main.ViewModels.ContactListViewModel.prototype.initItems;
	window.Main.ViewModels.ContactListViewModel.prototype.initItems = function (items) {
		const viewModel = this;
		const args = arguments;
		var queries = [];
		items.forEach(function(item) {
			item.AttributeFormProperties = window.ko.observableArray([]);
			if (window.database.CrmAttributeForms_AttributeForm) {
				queries.push({
					queryable: window.database.CrmAttributeForms_AttributeForm
						.includeDynamicFormElements()
						.include("DynamicForm.Languages")
						.include("Responses")
						.filter("it.ReferenceKey === this.id", { id: window.ko.unwrap(item.Id) })
						.filter("!it.Completed"),
					method: "toArray",
					handler: function(attributeForms) {
						window.Helper.Culture.languageCulture().then(function(language) {
							attributeForms.forEach(function(attributeForm) {
								var dynamicForm = attributeForm.DynamicForm;
								if(!dynamicForm)
									return;
								var elements = dynamicForm.Elements.filter(function(x) {
									return x.ExtensionValues.DisplayOnItemTemplate;
								});
								elements = elements.sort(function(a, b) { return a.SortOrder - b.SortOrder; });
								if (elements.length === 0)
									return;
								var fakeViewModel = {
									formReference: window.ko.observable(attributeForm.asKoObservable()),
									DynamicFormElementRules: [],
									findResponseObject: window.Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.findResponseObject
								};
								elements.forEach(function(dynamicFormElement) {
									viewModel.addDynamicFormElementLocalizationQuery(dynamicForm, dynamicFormElement, language);
									dynamicFormElement = window.ko.wrap.fromJS(dynamicFormElement);
									window.Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.initFormElement.bind(fakeViewModel)(dynamicFormElement);
									item.AttributeFormProperties.push(dynamicFormElement);
								});
							});
						});
					}
				});
			}
		});
		return Helper.Batch.Execute(queries)
			.then(function() {
				return viewModel.fetchDynamicFormLocalizations();
			})
			.then(function() {
				return baseInitItems.apply(viewModel, args);
			});
	};

	var baseApplyFilter = window.Main.ViewModels.ContactListViewModel.prototype.applyFilters;
	window.Main.ViewModels.ContactListViewModel.prototype.applyFilters = function(query) {
		var viewModel = this;
		query = baseApplyFilter.call(viewModel, query);
		return query.filter("filterByAttributeForm", { filters: window.ko.unwrap(viewModel.attributeFormFilters) });
	};

	var baseResetFilter = window.Main.ViewModels.ContactListViewModel.prototype.resetFilter;
	window.Main.ViewModels.ContactListViewModel.prototype.resetFilter = function () {
		var viewModel = this;
		viewModel.attributeFormFilters().forEach(function(element) {
			element.Response(null);
		});
		return baseResetFilter.call(viewModel);
	};

	window.Main.ViewModels.ContactListViewModel.prototype.fetchDynamicFormLocalizations = function() {
		var viewModel = this;
		return Helper.Batch.Execute(viewModel.dynamicFormsLocalizationQueries)
			.then(function() {
				return Helper.Batch.Execute(viewModel.localizationQueries);
			})
			.then(function () {
				viewModel.dynamicFormsLocalizationQueries = [];
				viewModel.localizationQueries = [];
			});
	};

	window.Main.ViewModels.ContactListViewModel.prototype.addDynamicFormElementLocalizationQuery = function (dynamicForm, dynamicFormElement, language) {
		var viewModel = this;
		var formId = dynamicForm.Id;
		var dynamicFormElementId = dynamicFormElement.Id;
		var defaultLanguage = dynamicForm.DefaultLanguageKey;
		viewModel.dynamicFormsLocalizationQueries.push({
			queryable: window.database.CrmDynamicForms_DynamicFormLanguage
				.filter(function (x) {
					return x.DynamicFormKey === this.dynamicFormId && x.LanguageKey === this.language && x.StatusKey === "Released";
				}, { dynamicFormId: formId, language: language }),
			method: "count",
			handler: function (results) {
				var exist = viewModel.localizations().filter(function (x) {
					return x.dynamicFormElementId === this.dynamicFormElementId && x.Language === this.language;
				}, { dynamicFormElementId: dynamicFormElementId, language: results === 1 ? language : defaultLanguage }).length > 0;
				if (!exist) {
					viewModel.localizationQueries.push({
						queryable: window.database.CrmDynamicForms_DynamicFormLocalization
							.filter(function (x) {
								return x.DynamicFormId === this.dynamicFormId && x.DynamicFormElementId === this.dynamicFormElementId && x.Language === this.language;
							}, { dynamicFormId: formId, dynamicFormElementId: dynamicFormElementId, language: results === 1 ? language : defaultLanguage }),
						method: "toArray",
						handler: function (localizations) {
							window.ko.utils.arrayPushAll(viewModel.localizations(), localizations);
						}
					});
				}
			}
		});
	};

	window.Main.ViewModels.ContactListViewModel.prototype.getLocalizationText = function (dynamicFormElement, choiceIndex, hint) {
		var viewModel = this;
		return window.Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.getLocalizationText.bind(viewModel)(
			dynamicFormElement,
			choiceIndex,
			false);
	};
})();