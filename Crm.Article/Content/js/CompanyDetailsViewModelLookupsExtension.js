(function () {
	var baseViewModel = window.Main.ViewModels.CompanyDetailsViewModel;
	window.Main.ViewModels.CompanyDetailsViewModel = function () {
		const viewModel = this;
		baseViewModel.apply(viewModel, arguments);
		viewModel.lookups.articleTypes = {$tableName: "CrmArticle_ArticleType"}
		viewModel.lookups.currencies = {$tableName: "Main_Currency"}
		viewModel.lookups.articleGroups01 = {$tableName: "CrmArticle_ArticleGroup01"}
		viewModel.lookups.articleGroups02 = {$tableName: "CrmArticle_ArticleGroup02"}
		viewModel.lookups.articleGroups03 = {$tableName: "CrmArticle_ArticleGroup03"}
		viewModel.lookups.articleGroups04 = {$tableName: "CrmArticle_ArticleGroup04"}
		viewModel.lookups.articleGroups05 = {$tableName: "CrmArticle_ArticleGroup05"}
	}
	window.Main.ViewModels.CompanyDetailsViewModel.prototype = baseViewModel.prototype;
})();