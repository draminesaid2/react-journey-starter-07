(function () {
	var baseViewModel = window.Main.ViewModels.CompanyDetailsRelationshipsTabViewModel;
	window.Main.ViewModels.CompanyDetailsRelationshipsTabViewModel = function () {
		const viewModel = this;
		baseViewModel.apply(viewModel, arguments);
		const documentAttributes = {
			Selector: "Parent.DocumentAttributes",
			Operation: "filter(function(x) { x.UseForThumbnail === null || x.UseForThumbnail === true })"
		}
		viewModel.genericArticleCompanyRelationships = new window.Main.ViewModels.GenericListViewModel("CrmArticle_ArticleCompanyRelationship", ["RelationshipTypeKey"], ["ASC"], ["Parent", "Parent.Tags", documentAttributes, "Parent.DocumentAttributes.FileResource", "Parent.ProductFamily"]);
		viewModel.genericArticleCompanyRelationships.getFilter("ChildId").extend({filterOperator: "==="})(viewModel.companyId);
		viewModel.articleCompanyRelationships = viewModel.genericArticleCompanyRelationships.items;
		viewModel.articleCompanyRelationships.distinct("RelationshipTypeKey");

		viewModel.subViewModels.push(viewModel.genericArticleCompanyRelationships);

		viewModel.lookups = viewModel.lookups || {};

		viewModel.lookups.articleCompanyRelationshipTypes = { $tableName: "CrmArticle_ArticleCompanyRelationshipType"}
		viewModel.lookups.articleGroups01 = { $tableName: "CrmArticle_ArticleGroup01"}
		viewModel.lookups.articleGroups02 = { $tableName: "CrmArticle_ArticleGroup02"}
		viewModel.lookups.articleGroups03 = { $tableName: "CrmArticle_ArticleGroup03"}
		viewModel.lookups.articleGroups04 = { $tableName: "CrmArticle_ArticleGroup04"}
		viewModel.lookups.articleGroups05 = { $tableName: "CrmArticle_ArticleGroup05"}
		viewModel.lookups.articleTypes = { $tableName: "CrmArticle_ArticleType"}
	}
	var baseInit = baseViewModel.prototype.init;
	window.Main.ViewModels.CompanyDetailsRelationshipsTabViewModel.prototype = baseViewModel.prototype;
	
	window.Main.ViewModels.CompanyDetailsRelationshipsTabViewModel.prototype.init = function (){
		var viewModel = this;
		return Helper.Lookup.getLocalizedArrayMaps(viewModel.lookups).then(baseInit.bind(viewModel,arguments))
	};

})();