namespace("Crm.Article.ViewModels");
window.Crm.Article.ViewModels.ArticleListIndexViewModel = function () {
	var viewModel = this;
	viewModel.lookups = {
		articleGroups01: { $tableName: "CrmArticle_ArticleGroup01" },
		articleGroups02: { $tableName: "CrmArticle_ArticleGroup02" },
		articleGroups03: { $tableName: "CrmArticle_ArticleGroup03" },
		articleGroups04: { $tableName: "CrmArticle_ArticleGroup04" },
		articleGroups05: { $tableName: "CrmArticle_ArticleGroup05" },
		articleTypes: { $tableName: "CrmArticle_ArticleType" },
		currencies: { $tableName: "Main_Currency" }
	};
	var documentAttributes = {
		Selector: "DocumentAttributes",
		Operation: "filter(function(x) { x.UseForThumbnail === null || x.UseForThumbnail === true })"
	}
	window.Main.ViewModels.ContactListViewModel.call(this, "CrmArticle_Article", "ItemNo", "ASC", ["Tags", documentAttributes, "DocumentAttributes.FileResource", "ProductFamily"]);
	var thumbnailViewObject = {
		Key: "Thumbnails",
		Value: window.Helper.String.getTranslatedString("Thumbnails")
	};
	viewModel.viewModes.push(thumbnailViewObject);
	viewModel.pageSize(24);
};
window.Crm.Article.ViewModels.ArticleListIndexViewModel.prototype =
	Object.create(window.Main.ViewModels.ContactListViewModel.prototype);
window.Crm.Article.ViewModels.ArticleListIndexViewModel.prototype.applyFilter = function (query, filterValue, filterName) {
	var viewModel = this;
	if (filterName === "Description") {
		return window.Helper.Article.getArticleAutocompleteFilter(query, filterValue.Value, viewModel.currentUser().DefaultLanguageKey);
	} else {
		return window.Main.ViewModels.ContactListViewModel.prototype.applyFilter.apply(this, arguments);
	}
};
window.Crm.Article.ViewModels.ArticleListIndexViewModel.prototype.init = function () {
	var viewModel = this;
	return window.Main.ViewModels.ContactListViewModel.prototype.init.apply(viewModel, arguments)
		.then(function () { return window.Helper.Lookup.getLocalizedArrayMaps(viewModel.lookups); });
};
window.Crm.Article.ViewModels.ArticleListIndexViewModel.prototype.initItems = function (items) {
	var viewModel = this;
	return window.Main.ViewModels.ContactListViewModel.prototype.initItems.apply(this, arguments).then(function () {
		return window.Helper.Article.loadArticleDescriptions(items, viewModel.currentUser().DefaultLanguageKey);
	});
};
window.Crm.Article.ViewModels.ArticleListIndexViewModel.prototype.deleteArticle = function (article) {
	var viewModel = this;
	window.Helper.Confirm.confirmDelete().then(function () {
		viewModel.loading(true);
		return window.database.CrmArticle_ArticleDescription
			.filter(function (it) { return it.Key === this.Key; }, { Key: article.ItemNo() })
			.forEach(articleDescription => window.database.remove(articleDescription));
	}).then(function () {
		var entity = window.Helper.Database.getDatabaseEntity(article);
		window.database.remove(entity);
		return window.database.saveChanges();
	}).fail(function(e){
		viewModel.loading(false);
		let errorMessage = window.Helper.String.tryExtractErrorMessageValue(e.message) ?? e.message;
		window.swal(window.Helper.String.getTranslatedString("Error"), errorMessage, "error");
	});
};

