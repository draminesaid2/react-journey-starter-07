namespace("Crm.Article.ViewModels").ProductFamilyListIndexViewModel = function () {
	var viewModel = this;
	window.Main.ViewModels.ContactListViewModel.call(this, "CrmArticle_ProductFamily", "Name", "ASC", ["ParentProductFamily", "ResponsibleUserUser"])
	var activeBookmark = {
		Category: window.Helper.String.getTranslatedString("Filter"),
		Name: window.Helper.String.getTranslatedString("AllActive"),
		Key: "All",
		Expression: function (query) {
			return query.filter(function (it) { return it.StatusKey === "active"; });
		}
	};
	viewModel.bookmarks.push(activeBookmark);
	viewModel.bookmark(activeBookmark);
	viewModel.bookmarks.push({
		Category: window.Helper.String.getTranslatedString("Filter"),
		Name: window.Helper.String.getTranslatedString("All"),
		Key: "All",
		Expression: function (query) {
			return query;
		}
	});
	viewModel.bookmarks.push({
		Category: window.Helper.String.getTranslatedString("Filter"),
		Name: window.Helper.String.getTranslatedString("Draft"),
		Key: "Draft",
		Expression: function (query) {
			return query.filter(function (it) { return it.StatusKey === "draft"; });
		}
	});
	viewModel.bookmarks.push({
		Category: window.Helper.String.getTranslatedString("Filter"),
		Name: window.Helper.String.getTranslatedString("RetiredProducts"),
		Key: "Retired",
		Expression: function (query) {
			return query.filter(function (it) { return it.StatusKey === "retired"; });
		}
	});
	viewModel.lookups = {
		productFamilyStatuses: { $tableName: "CrmArticle_ProductFamilyStatus" },
		ProductFamilyDescription: { $tableName: "CrmArticle_ProductFamilyStatus" }
	}
};
namespace("Crm.Article.ViewModels").ProductFamilyListIndexViewModel.prototype = Object.create(window.Main.ViewModels.ContactListViewModel.prototype);
namespace("Crm.Article.ViewModels").ProductFamilyListIndexViewModel.prototype.applyFilter = function (query, filterValue, filterName) {
	var viewModel = this;
	if (filterName === "Name") {
		return window.Helper.ProductFamily.getProductFamilyAutocompleteFilter(query, filterValue, viewModel.currentUser().DefaultLanguageKey);
	} else {
		return window.Main.ViewModels.ContactListViewModel.prototype.applyFilter.apply(this, arguments);
	}
};
namespace("Crm.Article.ViewModels").ProductFamilyListIndexViewModel.prototype.init = function () {
	var viewModel = this;
	return window.Helper.Lookup.getLocalizedArrayMaps(viewModel.lookups)
		.then(function () {
			return window.Main.ViewModels.ContactListViewModel.prototype.init.apply(viewModel, arguments);
		})
};
namespace("Crm.Article.ViewModels").ProductFamilyListIndexViewModel.prototype.initItems = function (items) {
	var viewModel = this;
	return window.Main.ViewModels.ContactListViewModel.prototype.initItems.apply(this, arguments).then(function () {
		return window.Helper.ProductFamily.loadProductFamilyDescriptions(items, viewModel.currentUser().DefaultLanguageKey);
	})
	
};
namespace("Crm.Article.ViewModels").ProductFamilyListIndexViewModel.prototype.applyFilter = function (query, filterValue, filterName) {
	var viewModel = this;
	if (filterName === "Descriptions") {
		return window.Helper.ProductFamily.getProductFamilyAutocompleteFilter(query, filterValue.Value, viewModel.currentUser().DefaultLanguageKey);
	} else {
		return window.Main.ViewModels.ContactListViewModel.prototype.applyFilter.apply(this, arguments);
	}
};
