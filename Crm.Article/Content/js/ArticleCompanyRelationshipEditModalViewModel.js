(function(ko) {
	const baseViewModel = window.Main.ViewModels.BaseRelationshipEditModalViewModel;
	namespace("Crm.Article.ViewModels").ArticleCompanyRelationshipEditModalViewModel = function(parentViewModel) {
		baseViewModel.apply(this, arguments);
		const viewModel = this;
		viewModel.table = window.database.CrmArticle_ArticleCompanyRelationship;
		viewModel.lookups = parentViewModel.tabs()["tab-relationships"]().lookups;
		viewModel.relationshipTypeLookup = viewModel.lookups.articleCompanyRelationshipTypes;
		viewModel.setMode(parentViewModel);
	};
	namespace("Crm.Article.ViewModels").ArticleCompanyRelationshipEditModalViewModel.prototype = Object.create(baseViewModel.prototype);
	namespace("Crm.Article.ViewModels").ArticleCompanyRelationshipEditModalViewModel.prototype.setMode = function(parentViewModel) {
		if (parentViewModel instanceof window.Main.ViewModels.CompanyDetailsViewModel) {
			this.mode = "company";
		} else if (parentViewModel instanceof window.Crm.Article.ViewModels.ArticleDetailsViewModel) {
			this.mode = "article";
		}
	};
	namespace("Crm.Article.ViewModels").ArticleCompanyRelationshipEditModalViewModel.prototype.init = function (id, params) {
		var viewModel = this;
		viewModel.contactType = params.contactType || "";
		viewModel.showCustomPersonSelector(true);
		baseViewModel.prototype.init.apply(this, arguments);
	};
	namespace("Crm.Article.ViewModels").ArticleCompanyRelationshipEditModalViewModel.prototype.getQueryForEditing = function() {
		return window.database.CrmArticle_ArticleCompanyRelationship
	};
	namespace("Crm.Article.ViewModels").ArticleCompanyRelationshipEditModalViewModel.prototype.getEditableId = function(relationship) {
		switch (this.mode) {
			case "company": return relationship.ParentId;
			case "article": return relationship.ChildId;
		}
		return baseViewModel.prototype.getEditableId.apply(this, arguments);
	};
	namespace("Crm.Article.ViewModels").ArticleCompanyRelationshipEditModalViewModel.prototype.getAutoCompleterOptions = function () {
		switch (this.mode) {
			case "company":
				return {
					key: "Id",
					table: 'CrmArticle_Article',
					orderBy: ['Name'],
					mapDisplayObject: window.Helper.Article.mapArticleForSelect2Display
				}
			case "article":
				return { 
					table: "Main_Company", 
					orderBy: ["Name"], 
					key: "Id", 
					mapDisplayObject: window.Helper.Company.mapForSelect2Display, 
					customFilter: window.Helper.Company.getSelect2Filter 
				};
				
		}

		return baseViewModel.prototype.getAutoCompleterOptions.apply(this, arguments);
	};
	namespace("Crm.Article.ViewModels").ArticleCompanyRelationshipEditModalViewModel.prototype.getAutoCompleterCaption = function() {
		switch (this.mode) {
			case "company": return "Article";
			case "article": return "Company";
		}
		return baseViewModel.prototype.getAutoCompleterCaption.apply(this, arguments);
	};
	namespace("Crm.Article.ViewModels").ArticleCompanyRelationshipEditModalViewModel.prototype.createNewEntity = function() {
		let relationship = baseViewModel.prototype.createNewEntity.apply(this, arguments);
		switch (this.mode) {
			case "article":
				relationship.ParentId = this.contactId();
				break;
			case "company":
				relationship.ParentId = null;
				relationship.ChildId = this.contactId();
				break;
			default:
				break;
		}
		return relationship;
	};
})(ko);