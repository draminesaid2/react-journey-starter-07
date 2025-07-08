class HelperDynamicForm {
	static getChoicesArray(dynamicFormElement) {
		var result = [];
		for (var i = 0; i < dynamicFormElement.Choices(); i++) {
			result.push(i);
		}
		if (dynamicFormElement.Randomized()) {
			return _.shuffle(result);
		}
		return result;
	}

	static getCssFromLayout(layout) {
		switch (layout) {
			case 1:
				return 'col-md-12';
			case 2:
				return 'col-md-6';
			case 3:
				return 'col-md-4';
			case 4:
				return 'side-by-side';
			default:
				return 'col-md-12';
		}
	}

	static getDynamicFormLocalization(dynamicForm, localizations, language) {
		dynamicForm = ko.unwrap(dynamicForm);
		localizations = ko.unwrap(localizations || dynamicForm.Localizations);
		language = language || document.getElementById("meta.CurrentLanguage").content;
		let localization = window.ko.utils.arrayFirst(localizations, function (x) {
			return ko.unwrap(x.DynamicFormElementId) === null && ko.unwrap(x.Language) === language;
		});
		if (!localization) {
			localization = window.ko.utils.arrayFirst(localizations, function (x) {
				return ko.unwrap(x.DynamicFormElementId) === null && ko.unwrap(x.Language) === ko.unwrap(dynamicForm.DefaultLanguageKey);
			});
		}
		return localization || null;
	}

	static getDynamicFormVisitReportFilter(query, term) {
		query = query.filter(function (it) {
			return it.CategoryKey === "VisitReport";
		});
		query = query.filter(
			"filterByDynamicFormTitle",
			{
				filter: (term || ""),
				languageKey: document.getElementById("meta.CurrentLanguage").content,
				statusKey: 'Released'
			});
		return query;
	}

	static getDescription(dynamicForm, localizations, language) {
		const localization = HelperDynamicForm.getDynamicFormLocalization(dynamicForm, localizations, language);
		return localization ? ko.unwrap(localization.Hint) : null;
	}

	static getTitle(dynamicForm, localizations, language) {
		const localization = HelperDynamicForm.getDynamicFormLocalization(dynamicForm, localizations, language);
		return localization ? ko.unwrap(localization.Value) : null;
	}

	static mapForSelect2Display(dynamicForm) {
		return {
			id: dynamicForm.Id,
			item: dynamicForm,
			text: Helper.DynamicForm.getTitle(dynamicForm)
		};
	}

	static scrollToError(viewModel, page, errorIndex) {
		const modalPager = viewModel.pager;
		modalPager.goToPage(page);
		setTimeout(() => {
			const parentModal = $(".modal").first();
			const formElement = $(".has-error").eq(errorIndex);
			const ytop = formElement.offset().top + parentModal.scrollTop();
			parentModal.scrollTop(ytop - $(window).scrollTop());
			setTimeout(() => {
				formElement.children(".dtp-container, .fg-line").children(".form-control").focus();
			});
		});
	}
}

(window.Helper = window.Helper || {}).DynamicForm = HelperDynamicForm;