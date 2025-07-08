namespace("Crm.DynamicForms.ViewModels").DynamicFormElementRuleEditorViewModel = function(formDesignerViewModel, formElement) {
	var viewModel = this;
	viewModel.validate = window.ko.observable(false);
	viewModel.formElementRuleConditionAsObservable = function (formElementRuleCondition) {
		var entity = formElementRuleCondition.asKoObservable();
		entity.DynamicFormElementId.extend({
			required: {
				params: true,
				onlyIf: function() {
					return viewModel.validate();
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("DynamicFormElement"))
			}
		});
		entity.Filter.extend({
			required: {
				params: true,
				onlyIf: function() {
					return viewModel.validate();
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Filter"))
			}
		});
		entity.Value.extend({
			required: {
				params: true,
				onlyIf: function() {
					return viewModel.validate() && entity.Filter() && ["After", "Before", "BeginsWith", "Contains", "DoesNotContain", "EndsWith", "Equals", "FilesEqualTo", "FilesLessThan", "FilesMoreThan", "Greater", "Less", "NotEquals"].indexOf(entity.Filter()) !== -1;
				},
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("Value"))
			}
		});
		return entity;
	};
	var dynamicFormElementId = window.ko.unwrap(formElement.Id);
	viewModel.form = formDesignerViewModel.form;
	viewModel.getLocalizationText = formDesignerViewModel.getLocalizationText.bind(formDesignerViewModel);
	viewModel.removedConditions = window.ko.observableArray([]);
	viewModel.removedRules = window.ko.observableArray([]);
	viewModel.rules = window.ko.observableArray([]);
	formDesignerViewModel.rules()
		.filter(x => x.DynamicFormElementId() === dynamicFormElementId)
		.forEach(function(existingRule) {
			var rule = window.database.CrmDynamicForms_DynamicFormElementRule.CrmDynamicForms_DynamicFormElementRule.create(existingRule.innerInstance).asKoObservable();
			rule.Conditions(window.ko.unwrap(existingRule.Conditions).map(function(x) {
			var mapped = viewModel.formElementRuleConditionAsObservable(window.database.CrmDynamicForms_DynamicFormElementRuleCondition.CrmDynamicForms_DynamicFormElementRuleCondition.create(x.innerInstance));
			mapped.innerInstance.storeToken = x.innerInstance.storeToken;
			return mapped;
		}));
		viewModel.rules.push(rule);
	});
	viewModel.ruleType = window.ko.observable(viewModel.rules().length > 0 ? viewModel.rules()[0].Type() : "Show");
	viewModel.ruleType.subscribe(function(ruleType) {
		viewModel.rules().forEach(function(rule) {
			rule.Type(ruleType);
		});
	});
	viewModel.createFormElementRule = function() {
		var rule = window.database.CrmDynamicForms_DynamicFormElementRule.CrmDynamicForms_DynamicFormElementRule.create(
			{
				Conditions: [],
				DynamicFormId: formDesignerViewModel.form().Id(),
				DynamicFormElementId: dynamicFormElementId,
				Id: window.$data.createGuid().toString().toLowerCase(),
				MatchType: "All",
				Type: "Show"
			}).asKoObservable();
		viewModel.createFormElementRuleCondition(rule);
		viewModel.rules.push(rule);
	};
	viewModel.removeFormElementRule = function(rule) {
		rule.Conditions().forEach(function(condition) {
			viewModel.removedConditions.push(condition);
		});
		viewModel.removedRules.push(rule);
		viewModel.rules.remove(rule);
	};
	viewModel.createFormElementRuleCondition = function(rule, index) {
		index = window.ko.unwrap(index || 0) + 1;
		var condition = viewModel.formElementRuleConditionAsObservable(window.database.CrmDynamicForms_DynamicFormElementRuleCondition
			.CrmDynamicForms_DynamicFormElementRuleCondition.create({
				DynamicFormElementId: null,
				DynamicFormElementRuleId: rule.Id(),
				Id: window.$data.createGuid().toString().toLowerCase(),
				Filter: null,
				Value: null
			}));
		rule.Conditions.splice(index, 0, condition);
	};
	viewModel.removeFormElementRuleCondition = function(rule, condition) {
		viewModel.removedConditions.push(condition);
		rule.Conditions.remove(condition);
	};
	viewModel.selectableFormElements = formDesignerViewModel.formElements().filter(function(x) {
		return x.Id() !== dynamicFormElementId &&
			x.FormElementType() !== "Image" &&
			x.FormElementType() !== "Literal" &&
			x.FormElementType() !== "PageSeparator" &&
			x.FormElementType() !== "SectionSeparator";
	});
	viewModel.getSelectableConditionFilters = function(condition) {
		var conditionFilterValues = viewModel.getSelectableConditionFilterValues(condition);
		return conditionFilterValues.map(function(x) {
			return {
				Text: window.Helper.String.getTranslatedString("DynamicFormElementCondition" + x),
				Value: x
			};
		});
	};
	viewModel.getSelectableConditionFilterValues = function(condition) {
		var formElementType = viewModel.getConditionFormElementType(condition);
		if (!formElementType) {
			return [];
		}
		if (formElementType === "Date" || formElementType === "Time") {
			return ["Equals", "Before", "After", "Empty", "NotEmpty"];
		}
		if (formElementType === "CheckBoxList") {
			return ["Equals", "NotEquals", "Contains", "DoesNotContain", "Empty", "NotEmpty"];
		}
		if (formElementType === "DropDown" || formElementType === "RadioButtonList") {
			return ["Equals", "NotEquals", "Empty", "NotEmpty"];
		}
		if (formElementType === "FileAttachmentDynamicFormElement") {
			return ["FilesEqualTo", "FilesMoreThan", "FilesLessThan"];
		}
		if (formElementType === "SignaturePad" || formElementType === "SignaturePadWithPrivacyPolicy") {
			return ["Signed", "NotSigned"];
		}
		if (formElementType === "Number") {
			return ["Equals", "Greater", "Less", "Empty", "NotEmpty"];
		}
		if (formElementType === "MultiLineText" || formElementType === "SingleLineText") {
			return ["Equals", "NotEquals", "Contains", "DoesNotContain", "BeginsWith", "EndsWith", "Empty", "NotEmpty"];
		}
		throw "Unknown FormElementType: " + formElementType;
	};
	viewModel.getSelectedConditionFilterValues = function(condition) {
		if (condition.values) {
			return condition.values;
		}
		condition.values = ko.observableArray(condition.Value() ? condition.Value().split(",") : []);
		condition.values.subscribe(function(values) {
			condition.Value((values || []).join(",") || null);
		});
		return condition.values;
	};
	viewModel.getConditionFormElementType = function(condition) {
		var conditionFormElement = window.ko.utils.arrayFirst(formDesignerViewModel.formElements(), function(x) {
			return x.Id() === condition.DynamicFormElementId();
		});
		if (!conditionFormElement) {
			return null;
		}
		return conditionFormElement.FormElementType();
	};
	viewModel.getSelectableConditionValues = function(condition) {
		var conditionFormElement = window.ko.utils.arrayFirst(formDesignerViewModel.formElements(), function(x) {
			return x.Id() === condition.DynamicFormElementId();
		});
		if (!conditionFormElement) {
			return [];
		}
		var choicesArray = window.ko.utils.arrayMap(Helper.DynamicForm.getChoicesArray(conditionFormElement),
			function(choice) {
				return {
					Value: choice.toString(),
					Text: formDesignerViewModel.getLocalizationText(conditionFormElement, choice)
				};
			});
		choicesArray.unshift({ Value: null, Text: window.Helper.String.getTranslatedString("PleaseSelect") });
		return choicesArray;
	};

	viewModel.save = function() {
		viewModel.errors = window.ko.validation.group(viewModel.rules);
		viewModel.validate(true);
		if (viewModel.errors().length > 0) {
			viewModel.errors.showAllMessages();
			return;
		}
		viewModel.rules().forEach(function(ruleToSave) {
			var rule = formDesignerViewModel.getRule(formElement, ruleToSave);
			rule.Conditions(window.ko.unwrap(ruleToSave.Conditions));
			rule.MatchType(window.ko.unwrap(ruleToSave.MatchType));
			rule.Type(window.ko.unwrap(ruleToSave.Type));
			ruleToSave.Conditions().forEach(function(conditionToSave) {
				var condition = formDesignerViewModel.getCondition(conditionToSave);
				condition.DynamicFormElementId(window.ko.unwrap(conditionToSave.DynamicFormElementId));
				condition.Filter(window.ko.unwrap(conditionToSave.Filter));
				condition.Value(window.ko.unwrap(conditionToSave.Value));
			});
			rule.Conditions().forEach(function(condition) {
				window.Helper.Database.registerDependency(condition, rule);
			});
		});
		viewModel.removedConditions().forEach(function(conditionToRemove) {
			var removed = formDesignerViewModel.conditions.remove(function(x) {
				return x.Id() === conditionToRemove.Id();
			});
			if (removed.length > 0) {
				var rule = formDesignerViewModel.rules().find(function(x) {
					return x.Id() === conditionToRemove.DynamicFormElementRuleId();
				});
				rule.innerInstance.entityState === $data.EntityState.Added ? window.database.detach(conditionToRemove.innerInstance) : window.database.remove(conditionToRemove.innerInstance);
				window.Helper.Database.registerDependency(rule, conditionToRemove);
			}
		});
		viewModel.removedRules().forEach(function(ruleToRemove) {
			var removed = formDesignerViewModel.rules.remove(function(x) { return x.Id() === ruleToRemove.Id(); });
			if (removed.length > 0) {
				window.database.remove(removed[0].innerInstance);
			}
		});
		$("#dynamic-form-element-rule-editor").modal("hide");
	};
};