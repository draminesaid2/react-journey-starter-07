class HelperDynamicFormDesigner {
	static addChoice(dynamicFormElement, index) {
			let choices = dynamicFormElement.Choices();
			dynamicFormElement.Choices(0);
			if (Array.isArray(dynamicFormElement.Response())) {
				dynamicFormElement.Response.splice(index + 1, 0, ko.observable(false));
			}
			dynamicFormElement.Localizations()
				.filter(x => x.ChoiceIndex() > index)
				.forEach(localization => localization.ChoiceIndex(localization.ChoiceIndex() + 1));
			dynamicFormElement.Choices(choices + 1);
	}

	static displayAboutToBeReleasedWarning(dynamicForm, alreadySavedDynamicForm) {
		if (!dynamicForm || !dynamicForm.Languages()) {
			return false;
		}
		return dynamicForm.Languages().some(x => x.StatusKey() === "Released") &&
			alreadySavedDynamicForm.Languages().every(x => x.StatusKey() !== "Released");
	}

	static displayAlreadyReleasedWarning(alreadySavedDynamicForm) {
		if (!alreadySavedDynamicForm || !alreadySavedDynamicForm.Languages()) {
			return false;
		}
		return alreadySavedDynamicForm.Languages().some(x => x.StatusKey() === "Released");
	}

	static reducedFunctionalityModeActive() {
		return window.client.isMobileDevice();
	}

	static getCondition(form, rules, conditions, dynamicFormElementRuleCondition) {
			var condition = ko.utils.arrayFirst(conditions(), function (x) {
				return x.Id() === dynamicFormElementRuleCondition.Id();
			});
			if (!condition) {
				condition = window.database.CrmDynamicForms_DynamicFormElementRuleCondition.CrmDynamicForms_DynamicFormElementRuleCondition.create();
				condition.DynamicFormId = form().Id();
				condition.DynamicFormElementRuleId = ko.unwrap(dynamicFormElementRuleCondition.DynamicFormElementRuleId);
				condition.Id = ko.unwrap(dynamicFormElementRuleCondition.Id);
				var rule = rules().find(function (x) {
					return x.Id() === condition.DynamicFormElementRuleId;
				});
				window.Helper.Database.registerDependency(condition, rule);
				condition = condition.asKoObservable();
				conditions.push(condition);
			}
			return condition;
	}

	static getLocalization(form, dynamicFormElement, choiceIndex, language, localizations) {
			var localization;
			if (dynamicFormElement) {
				localization = dynamicFormElement.Localizations().find(function (x) {
					return x.ChoiceIndex() === choiceIndex && x.Language() === language;
				});
			} else {
				localization = ko.utils.arrayFirst(localizations,
					function (x) {
						return x.ChoiceIndex() === choiceIndex && x.Language() === language;
					});
			}
			if (!localization) {
				localization = window.database.CrmDynamicForms_DynamicFormLocalization.CrmDynamicForms_DynamicFormLocalization.create();
				localization.CreateDate = new Date();
				localization.CreateUser = Helper.User.getCurrentUserName();
				localization.DynamicFormId = form().Id();
				localization.DynamicFormElement = dynamicFormElement;
				localization.DynamicFormElementId = dynamicFormElement ? ko.unwrap(dynamicFormElement.Id) : null;
				localization.ChoiceIndex = choiceIndex;
				localization.Language = ko.unwrap(language);
				localization.ModifyDate = new Date();
				localization.ModifyUser = Helper.User.getCurrentUserName();
				localization.Value = "";
				if (dynamicFormElement) {
					window.Helper.Database.registerDependency(localization, dynamicFormElement);
				}
				window.database.add(localization);
				localization = localization.asKoObservable();
				if (dynamicFormElement) {
					dynamicFormElement.Localizations.push(localization);
				} else {
					localizations.push(localization);
				}
			}
			return localization;
	}

	static getLocalizationText(dynamicFormElement, choiceIndex, hint, localizations, language) {
			var localization;
			if (dynamicFormElement) {
				localization = dynamicFormElement.Localizations().find(function (x) {
					return x.ChoiceIndex() === choiceIndex && x.Language() === language;
				});
			} else {
				localization = ko.utils.arrayFirst(localizations,
					function (x) {
						return x.ChoiceIndex() === choiceIndex && x.Language() === language;
					});
			}
			var defaultValue = "";
			if (hint === false && dynamicFormElement && (isNaN(choiceIndex) || choiceIndex === null)) {
				defaultValue = Helper.String.getTranslatedString(ko.unwrap(dynamicFormElement.FormElementType), ko.unwrap(dynamicFormElement.FormElementType));
			} else if (hint === false && dynamicFormElement) {
				defaultValue = (choiceIndex + 1).toString();
			}
			if (!localization) {
				return defaultValue;
			}
			var result = ko.unwrap(hint ? localization.Hint : localization.Value);
			return result || defaultValue;
	}

	static getReferencingElements(dynamicFormElement, conditions, rules, formElements) {
			var ruleIds = conditions.filter(function (x) {
				return x.DynamicFormElementId() === dynamicFormElement.Id();
			}).map(function (x) {
				return x.DynamicFormElementRuleId();
			});
			var formElementIds = rules.filter(function (x) {
				return ruleIds.indexOf(x.Id()) !== -1;
			}).map(function (x) {
				return x.DynamicFormElementId();
			});
			return formElements.filter(function (x) {
				return formElementIds.indexOf(x.Id()) !== -1;
			});
	}

	static getRule(dynamicForm, dynamicFormElement, dynamicFormElementRule, rules) {
			var rule = ko.utils.arrayFirst(rules(), function (x) {
				return x.Id() === dynamicFormElementRule.Id();
			});
			if (!rule) {
				rule = window.database.CrmDynamicForms_DynamicFormElementRule.CrmDynamicForms_DynamicFormElementRule.create();
				rule.DynamicFormElement = dynamicFormElement;
				rule.DynamicFormElementId = dynamicFormElement ? ko.unwrap(dynamicFormElement.Id) : null;
				rule.DynamicFormId = dynamicForm().Id();
				rule.Id = ko.unwrap(dynamicFormElementRule.Id);
				if (dynamicFormElement) {
					window.Helper.Database.registerDependency(rule, dynamicFormElement);
				}
				window.database.add(rule);
				rule = rule.asKoObservable();
				rules.push(rule);
			}
			return rule;
	}

	static hasRules(dynamicFormElement, rules) {
			var rule = ko.utils.arrayFirst(rules(), function (x) {
				return x.DynamicFormElementId() === dynamicFormElement.Id();
			});
			return !!rule;
	}

	static getSizeOptionList() {
		return ko.observableArray([
			{Id: 1, Name: Helper.String.getTranslatedString("Large1ElementRow")},
			{Id: 2, Name: Helper.String.getTranslatedString("Medium2ElementRow")},
			{Id: 4, Name: Helper.String.getTranslatedString("Small4ElementRow")}
		])
	}

	static getRowSizeOptionList() {
		return ko.observableArray([
			{Id: 1, Name: Helper.String.getTranslatedString("Large")},
			{Id: 2, Name: Helper.String.getTranslatedString("Medium")},
			{Id: 6, Name: Helper.String.getTranslatedString("Small")}
		])
	}

	static getLayoutOptionList() {
		return ko.observableArray([
			{Id: 1, Name: Helper.String.getTranslatedString("OneColumn")},
			{Id: 2, Name: Helper.String.getTranslatedString("TwoColumn")},
			{Id: 3, Name: Helper.String.getTranslatedString("ThreeColumn")},
			{Id: 4, Name: Helper.String.getTranslatedString("SideBySide")}
		])
	}

	static getSortableOptions() {
			return {
				connectWith: ".sortable-item",
				items: ".sortable-item",
				handle: ".move",
				tolerance: "pointer",
				placeholder: "panel panel-placeholder col-xs-3",
				start: function (e, ui) {
					let placeHolderWidth = ui.item.find(".panel").width() || $(ui.placeholder).parent().width() / 4 - 31;
					ui.placeholder.width(placeHolderWidth);
					let placeHolderHeight = ui.item.find(".panel").height() || 150;
					ui.placeholder.height(placeHolderHeight);
					ui.placeholder.addClass(ui.item.attr("class"));
				}
			};
	}

	static groupFormElements(elements) {
			var sections = [];
			var elementGroups = [];
			var currentGroup = [];
			var currentSize = 0;
			for (var i = 0; i < ko.unwrap(elements).length; i++) {
				var element = ko.unwrap(elements)[i];
				if (element.FormElementType() === "PageSeparator" || element.FormElementType() === "SectionSeparator") {
					if (currentGroup.length > 0) {
						elementGroups.push(currentGroup);
						currentGroup = [];
						currentSize = 0;
					}
					if (elementGroups.length > 0) {
						sections.push(elementGroups);
						elementGroups = [];
					}
				}
				if ((12 / ko.unwrap(element.Size)) + currentSize > 12) {
					elementGroups.push(currentGroup);
					currentGroup = [];
					currentSize = 0;
				}
				currentGroup.push(element);
				currentSize += (12 / ko.unwrap(element.Size));
			}
			if (currentGroup.length > 0) {
				elementGroups.push(currentGroup);
			}
			if (elementGroups.length > 0) {
				sections.push(elementGroups);
			}
			if (sections.length === 0) {
				sections.push([[]]);
			}
			return sections;
	}

	static removeChoice(dynamicFormElement, index) {
			dynamicFormElement.Choices(dynamicFormElement.Choices() - 1);
			if (Array.isArray(dynamicFormElement.Response())) {
				dynamicFormElement.Response.splice(index, 1);
				if (dynamicFormElement.MaxChoices() > dynamicFormElement.Choices()) {
					dynamicFormElement.MaxChoices(dynamicFormElement.Choices());
					if (dynamicFormElement.MaxChoices() < dynamicFormElement.MinChoices()) {
						dynamicFormElement.MinChoices(dynamicFormElement.MaxChoices());
					}
				}
				if (dynamicFormElement.MinChoices() > dynamicFormElement.Choices()) {
					dynamicFormElement.MinChoices(dynamicFormElement.Choices());
				}
			}
			let localizationsToRemove = dynamicFormElement.Localizations().filter(x => x.ChoiceIndex() === index);
			dynamicFormElement.Localizations()
				.filter(x => x.ChoiceIndex() > index)
				.sort((a, b) => b.ChoiceIndex() - a.ChoiceIndex())
				.forEach(localization => localization.ChoiceIndex(localization.ChoiceIndex() - 1));
			dynamicFormElement.Localizations.removeAll(localizationsToRemove);
			localizationsToRemove.forEach(localization => {
				if (localization.innerInstance.entityState === $data.EntityState.Added) {
					localization.Id(new Date().getTime());
					window.database.detach(localization.innerInstance);
				} else {
					window.database.remove(localization.innerInstance);
				}
			});
	}

	static removeFormElement(formElement, formElements, rules, conditions) {
			window.database.remove(formElement.innerInstance);
			var index = formElements().indexOf(formElement);
			formElements().splice(index, 1);
			formElements.valueHasMutated();

			if (formElement.innerInstance instanceof window.database.CrmDynamicForms_Image.elementType &&
				formElement.FileResourceId()) {
				var oldFileResource =
					window.database.Main_FileResource.attachOrGet({Id: formElement.FileResourceId()});
				window.database.Main_FileResource.remove(oldFileResource);
				window.Helper.Database.registerDependency(oldFileResource, formElement);
			}

			formElement.Localizations.removeAll().forEach(function (removedLocalization) {
				window.database.remove(removedLocalization.innerInstance);
				window.Helper.Database.registerDependency(formElement, removedLocalization);
			});

			var removedRules = [];
			rules.remove(function (rule) {
				return rule.DynamicFormElementId() === formElement.Id();
			}).forEach(function (rule) {
				window.database.remove(rule.innerInstance);
				window.Helper.Database.registerDependency(formElement, rule);
				removedRules.push(rule);
			});

			conditions.remove(function (condition) {
				return condition.DynamicFormElementId() === formElement.Id() ||
					removedRules.map(function (x) {
						return x.Id();
					})
						.indexOf(condition.DynamicFormElementRuleId()) !==
					-1;
			}).forEach(function (condition) {
				var rule = removedRules.concat(rules()).find(function (x) {
					return x.Id() === condition.DynamicFormElementRuleId();
				});
				rule.Conditions.remove(x => x.Id() === condition.Id());
				rule.innerInstance.entityState === $data.EntityState.Added ? window.database.detach(condition.innerInstance) : window.database.remove(condition.innerInstance);
				window.Helper.Database.registerDependency(rule, condition);
			});

			rules.remove(function (rule) {
				return rule.Conditions().length === 0;
			}).forEach(function (rule) {
				window.database.remove(rule.innerInstance);
				window.Helper.Database.registerDependency(formElement, rule);
			});
		}
}

(window.Helper = window.Helper || {}).DynamicFormDesigner = HelperDynamicFormDesigner;