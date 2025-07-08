(function () {
	var baseInit = window.Main.ViewModels.ContactDetailsViewModel.prototype.init;
	window.Main.ViewModels.ContactDetailsViewModel.prototype.init = function (id) {
		var viewModel = this;
		if (window.database.CrmAttributeForms_AttributeForm) {
			window.Helper.Database.registerEventHandlers(viewModel,
				{
					CrmAttributeForms_AttributeForm: {
						afterCreate: function() {
							viewModel.loading(true);
							viewModel.updateAttributeForms().then(function() {
								viewModel.loading(false);
							});
						}
					}
				});
		}
		viewModel.attributeForms = window.ko.observableArray([]);
		return baseInit.apply(this, arguments)
			.then(function() {
				return viewModel.updateAttributeForms();
			});
	};

	var baseDispose = window.Main.ViewModels.ContactDetailsViewModel.prototype.dispose;
	window.Main.ViewModels.ContactDetailsViewModel.prototype.dispose = function(id) {
		if ($.isFunction(baseDispose)) {
			baseDispose.apply(this, arguments);
		}
	};
	window.Main.ViewModels.ContactDetailsViewModel.prototype.deleteAttributeForm = function () {
		var viewModel = this;
		window.Helper.Confirm.confirmDelete().done(function() {
			viewModel.loading(true);
			window.database.CrmAttributeForms_AttributeForm
				.include("Responses")
				.filter(function (it) {
					return it.ReferenceKey === this.contactKey && it.DynamicFormKey === this.dynamicFormKey;
				}, { contactKey: viewModel.formReference().ReferenceKey(), dynamicFormKey: viewModel.formReference().DynamicFormKey() })
				.toArray()
				.then(function (attributeForms) {
					attributeForms.forEach(function (attributeForm) {
						attributeForm.Responses.forEach(function (response) {
							window.database.remove(response);
						});
						attributeForm.Responses = [];
						window.database.remove(attributeForm);
					});
					window.database.saveChanges().then(function() {
						return viewModel.parentViewModel.updateAttributeForms();
					}).then(function() {
						viewModel.loading(false);
					});
				});
		});
	};
	window.Main.ViewModels.ContactDetailsViewModel.prototype.updateAttributeForms = function () {
		var viewModel = this;
		var d = new $.Deferred();
		if (!window.database.CrmAttributeForms_AttributeForm) {
			return d.resolve().promise();
		}
		window.database.CrmAttributeForms_AttributeForm
						.includeDynamicFormElements()
						.include("DynamicForm.Languages")
						.include("Responses")
						.filter("it.ReferenceKey === this.contactId && it.Completed === false", { contactId: viewModel.contactId() })
						.orderBy("it.CreateDate")
						.toArray()
						.then(function (attributeForms) {
							var attributeFormsViewModels = [];
							window.async.eachSeries(attributeForms, function (attributeForm, cb) {
								var dynamicFormDetailsViewModel = new window.Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel();
								
								dynamicFormDetailsViewModel.applyChanges = function() {
									return window.database.saveChanges();
								};
								dynamicFormDetailsViewModel.parentViewModel = viewModel;
								dynamicFormDetailsViewModel.init({ formReference: attributeForm.asKoObservable() }).then(function() {
									dynamicFormDetailsViewModel.errors = window.ko.observableArray([]);
									dynamicFormDetailsViewModel.formReference().Responses().forEach(function(response) {
										window.database.detach(response);
									});
									dynamicFormDetailsViewModel.formReference().Responses([]);
									window.database.detach(dynamicFormDetailsViewModel.formReference().innerInstance);
									attributeFormsViewModels.push(dynamicFormDetailsViewModel);
									dynamicFormDetailsViewModel.loading(false);
									cb();
								});
							}, function() {
								attributeFormsViewModels.forEach(function(dynamicFormDetailsViewModel) {
									dynamicFormDetailsViewModel.loading.subscribe(function() {
										viewModel.loading(attributeFormsViewModels.some(x => x.loading()));
									});
								});
								viewModel.attributeForms(attributeFormsViewModels);
								d.resolve();
							});
						});
		return d.promise();
	};
	window.Main.ViewModels.ContactDetailsViewModel.prototype.attributeFormReset = function (pmbbViewModel) {
		var viewModel = this;

		if (pmbbViewModel.elements === undefined)
			pmbbViewModel.elements = window.ko.observableArray();

		pmbbViewModel.elements(window.ko.unwrap(window.ko.copy(viewModel.elements)).map(function (element) {
			element.Response(window.ko.toJS(window.ko.unwrap(element.Response)));
			element.OriginalResponse = window.ko.observable(window.ko.toJS(window.ko.unwrap(element.Response)));
			element.isVisible = window.ko.observable(true);
			return element;
		}));
		viewModel.elements().forEach(function(element) {
			if(element.Response.rules) {
				element.Response.rules([]);
			}
		});
		pmbbViewModel.validationRulesAdded = window.ko.observable(false);
		pmbbViewModel.requiredValidationRulesAdded = window.ko.observable(false);
		window.Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.addValidationRules.bind(pmbbViewModel)();
		window.Crm.DynamicForms.ViewModels.DynamicFormDetailsViewModel.prototype.addRequiredValidationRules.bind(pmbbViewModel)();
	};
	window.Main.ViewModels.ContactDetailsViewModel.prototype.attributeFormBeforeSave = function(pmbbViewModel) {
		pmbbViewModel.errors = window.ko.validation.group(pmbbViewModel.elements(), { deep: true });
	};
	window.Main.ViewModels.ContactDetailsViewModel.prototype.attributeFormSave = function (pmbbViewModel) {
		var viewModel = this;
		var isUpdated = false;
		var newAttributeForm = window.database.CrmAttributeForms_AttributeForm.CrmAttributeForms_AttributeForm.create();
		window.database.add(newAttributeForm);
		window.Helper.Database.transferData(
			["DynamicFormKey", "ReferenceKey"],
			viewModel.formReference().innerInstance,
			newAttributeForm);
		viewModel.elements().forEach(function (element) {
			var editElement = pmbbViewModel.elements().find(function (x) { return x.Id() === element.Id(); });
			var elementType = window.ko.unwrap(element.FormElementType);
			var oldResponse = window.ko.unwrap(editElement.OriginalResponse);
			var newResponse = window.ko.unwrap(editElement.Response);
			if ((newResponse || "").toString() !== (oldResponse || "").toString()) {
				isUpdated = true;
			}

			var response = window.database.CrmDynamicForms_DynamicFormResponse.CrmDynamicForms_DynamicFormResponse.create();
			response.DynamicFormReferenceKey = newAttributeForm.Id;
			response.DynamicFormElementKey = element.Id();
			response.DynamicFormElementType = elementType;
			response.DynamicFormKey = viewModel.DynamicForm().Id();
			response.Value = newResponse;
			window.database.add(response);
			newAttributeForm.Responses.push(response);
		});

		if (!isUpdated) {
			newAttributeForm.Responses.forEach(function(response) {
				window.database.detach(response);
			});
			window.database.detach(newAttributeForm);
		} else {
			window.database.attachOrGet(viewModel.formReference().innerInstance);
			viewModel.formReference().Completed(true);
			newAttributeForm.Responses = [];
		}
	};
})();