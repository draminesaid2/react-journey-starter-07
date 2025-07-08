namespace("Crm.DynamicForms.ViewModels").DynamicFormLocalizationEditorViewModel = function (form, dynamicFormElement, choiceIndex, isMultiline, isHint, languages, localizations) {
	const viewModel = this;
	choiceIndex = isNaN(choiceIndex) ? null : choiceIndex;
	const dynamicFormElementId = dynamicFormElement ? window.ko.unwrap(dynamicFormElement.Id) : null;
	isMultiline = isMultiline === true;
	viewModel.init = async function init() {
		const results = await window.database.CrmDynamicForms_DynamicFormLocalization.filter(function (x) {
			return x.IsActive === true && x.DynamicFormId === this.dynamicFormId && x.DynamicFormElementId === this.dynamicFormElementId && x.ChoiceIndex === this.choiceIndex;
		}, {
			dynamicFormId: form.Id(),
			dynamicFormElementId: dynamicFormElementId,
			choiceIndex: choiceIndex
		}).toArray();
		viewModel.form = form;
		viewModel.languages = languages;
		viewModel.localizations = ko.observableArray(results.map(x => x.asKoObservable()));
		viewModel.multiline = ko.observable(isMultiline);
		viewModel.isHint = ko.observable(isHint);
		viewModel.form.Languages().forEach((language) => {
			if (results.filter(function (x) {
				return ko.unwrap(x.Language) === ko.unwrap(language.LanguageKey);
			}).length === 0) {
				const localization = window.database.CrmDynamicForms_DynamicFormLocalization.CrmDynamicForms_DynamicFormLocalization.create();
				localization.CreateDate = new Date();
				localization.CreateUser = Helper.User.getCurrentUserName();
				localization.DynamicFormId = form.Id();
				localization.DynamicFormElement = dynamicFormElement;
				localization.DynamicFormElementId = dynamicFormElementId;
				localization.ChoiceIndex = choiceIndex;
				localization.entityState = window.$data.EntityState.Added;
				localization.Language = ko.unwrap(language.LanguageKey);
				localization.ModifyDate = new Date();
				localization.ModifyUser = Helper.User.getCurrentUserName();
				localization.Value = "";
				if (dynamicFormElement) {
					Helper.Database.registerDependency(localization, dynamicFormElement);
				}
				viewModel.localizations.push(localization.asKoObservable());
			}
		});
		viewModel.localizations().forEach((localization) => {
			let existingLocalization;
			if (dynamicFormElement) {
				existingLocalization = dynamicFormElement.Localizations().find(function (x) {
					return x.ChoiceIndex() === choiceIndex && x.Language() === localization.Language();
				});
			} else {
				existingLocalization = ko.utils.arrayFirst(localizations,
					function (x) {
						return x.ChoiceIndex() === choiceIndex && x.Language() === localization.Language();
					});
			}
			if (existingLocalization) {
				localization.Hint(ko.unwrap(existingLocalization.Hint));
				localization.Value(ko.unwrap(existingLocalization.Value));
			}
		});
		viewModel.localizations.sort(function (x, y) {
			return x.Language().localeCompare(y.Language());
		});
	}
};