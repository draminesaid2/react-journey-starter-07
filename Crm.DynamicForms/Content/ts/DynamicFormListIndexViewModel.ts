///<reference path="../../../../Content/@types/index.d.ts" />
import {namespace} from "../../../../Content/ts/namespace";

class DynamicFormListIndexViewModel extends window.Main.ViewModels.GenericListViewModel {
	lookups: any;

	constructor() {
		super(
			"CrmDynamicForms_DynamicForm",
			"Title",
			"ASC",
			["Languages"]
		);
		this.lookups = {
			dynamicFormCategories: {$tableName: "CrmDynamicForms_DynamicFormCategory"},
			dynamicFormStatuses: {$tableName: "CrmDynamicForms_DynamicFormStatus"},
			languages: {$tableName: "Main_Language"}
		};
		this.addBookmarks();
	}

	addBookmarks(): void {
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("All"),
			Key: "All",
			Expression: function (query) {
				return query;
			}
		});
		const activeBookmark = {
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("Active"),
			Key: "Active",
			Expression: function (query) {
				return query.filter("it.Languages.some(function(it2){ return it2.StatusKey !== this.disabledStatusKey; })", {disabledStatusKey: "Disabled"});
			}
		};
		this.bookmarks.push(activeBookmark);
		this.bookmark(activeBookmark);
		this.bookmarks.push({
			Category: window.Helper.String.getTranslatedString("Filter"),
			Name: window.Helper.String.getTranslatedString("Released"),
			Key: "Released",
			Expression: function (query) {
				return query.filter("it.Languages.some(function(it2){ return it2.StatusKey === this.releasedStatusKey; })", {releasedStatusKey: "Released"});
			}
		});
	}

	applyFilters(query): void {
		let titleFilter = null;
		if (ko.unwrap(this.filters["Title"])) {
			titleFilter = this.filters["Title"];
			query = query.filter("it.Localizations.some(function(it2){ return it2.DynamicFormElementId === null && it2.Value.contains(this.filter); })", {filter: titleFilter().Value});
			delete this.filters["Title"];
		}
		let descriptionFilter = null;
		if (ko.unwrap(this.filters["Description"])) {
			descriptionFilter = this.filters["Description"];
			query = query.filter("it.Localizations.some(function(it2){ return it2.DynamicFormElementId === null && it2.Hint.contains(this.filter); })", {filter: descriptionFilter().Value});
			delete this.filters["Description"];
		}
		query = super.applyFilters(query);
		if (titleFilter) {
			this.filters["Title"] = titleFilter;
		}
		if (descriptionFilter) {
			this.filters["Description"] = descriptionFilter;
		}
		return query;
	}

	applyOrderBy(query): void {
		if (this.orderBy() === "Title") {
			query = query.orderBy("orderByDynamicFormTitle", {
				language: this.currentUser().DefaultLanguageKey,
				direction: this.orderByDirection()
			})
		} else {
			query = super.applyOrderBy(query);
		}
		return query;
	}

	async remove(dynamicForm) {
		try {
			await window.Helper.Confirm.confirmDelete();
		} catch {
			return;
		}
		this.loading(true);
		const usage = await fetch(window.Helper.Url.resolveUrl("~/Crm.DynamicForms/DynamicForm/CheckDynamicFormUsage?dynamicFormId=" + dynamicForm.Id())).then(r => r.json());
		if (usage.isUsed) {
			this.loading(false);
			try {
				await window.Helper.Confirm.genericConfirm({
					text: window.Helper.String.getTranslatedString('DisableDynamicFormQuestion'),
					type: "info"
				});
			} catch {
				return;
			}
		}
		this.loading(true);
		let data = new FormData();
		data.append("dynamicFormId", dynamicForm.Id());
		await fetch(window.Helper.Url.resolveUrl("~/Crm.DynamicForms/DynamicForm/Delete"), {
			method: "POST",
			body: data
		});
		if (ko.unwrap(this.items).length === 1 && this.page() > 1) {
			let prevPage = this.page() - 1;
			this.page(prevPage);
			this.currentSearch = this.search(true, true);
		} else {
			this.currentSearch = this.search(false, true);
		}
	}

	getCategoryAbbreviation(dynamicForm): string {
		const dynamicFormCategoryKey = dynamicForm.CategoryKey();
		if (!dynamicFormCategoryKey) {
			return "";
		}
		const dynamicFormCategory = this.lookups.dynamicFormCategories[dynamicFormCategoryKey];
		if (!!dynamicFormCategory && !!dynamicFormCategory.Value) {
			return dynamicFormCategory.Value[0];
		}
		return dynamicFormCategoryKey[0];
	};

	getColor(dynamicForm): string {
		const defaultColor = "#9E9E9E";
		if (dynamicForm.Languages().length === 0) {
			return defaultColor;
		}
		if (dynamicForm.Languages().every(x => x.StatusKey() === "Released")) {
			return "#4CAF50"
		}
		if (dynamicForm.Languages().some(x => x.StatusKey() === "Released")) {
			return "#FF9800"
		}
		if (dynamicForm.Languages().some(x => x.StatusKey() === "Disabled")) {
			return "#F44336"
		}
		return defaultColor;
	};

	async init(id, params): Promise<void> {
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		await super.init(id, params);
	}

	initItems(items): JQuery.Deferred<any> {
		return super.initItems(items).then(() => {
			let dynamicFormIds = items.map(x => x.Id());
			return window.database.CrmDynamicForms_DynamicFormLocalization
				.filter("it.DynamicFormId in this.dynamicFormIds && it.DynamicFormElementId === null", {dynamicFormIds: dynamicFormIds})
				.toArray();
		}).then(localizations => {
			items.forEach(item => {
				item.Localizations(localizations.filter(x => x.DynamicFormId === item.Id()));
			});
			return items;
		});
	}
}

namespace("Crm.DynamicForms.ViewModels").DynamicFormListIndexViewModel = DynamicFormListIndexViewModel;