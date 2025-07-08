namespace("Crm.Article.ViewModels").ArticleCreateViewModel = function () {
	var viewModel = this;
	viewModel.status = window.ko.observable(window.Helper.Offline ? window.Helper.Offline.status : "online");
	viewModel.loading = window.ko.observable(true);
	viewModel.article = window.ko.observable(null);
	viewModel.errors = window.ko.validation.group(viewModel.article, { deep: true });
	viewModel.translation = window.ko.observable(false);
	viewModel.descriptions = window.ko.observableArray();
	viewModel.articleDescriptions = window.ko.observableArray();
	viewModel.articles = window.ko.observableArray();
	viewModel.lookups = {
		articleTypes: { $tableName: "CrmArticle_ArticleType" },
		currencies: { $tableName: "Main_Currency" },
		quantityUnits: { $tableName: "CrmArticle_QuantityUnit" },
		vatLevels: { $tableName: "CrmArticle_VATLevel" },
		articleGroups01: { $tableName: "CrmArticle_ArticleGroup01" },
		articleGroups02: { $tableName: "CrmArticle_ArticleGroup02" },
		articleGroups03: { $tableName: "CrmArticle_ArticleGroup03" },
		articleGroups04: { $tableName: "CrmArticle_ArticleGroup04" },
		articleGroups05: { $tableName: "CrmArticle_ArticleGroup05" },
		languages: { $tableName: "Main_Language", $filterExpression: "it.IsSystemLanguage === true" }
	};
};

namespace("Crm.Article.ViewModels").ArticleCreateViewModel.prototype.articleFactories = {}
namespace("Crm.Article.ViewModels").ArticleCreateViewModel.prototype.initNewArticle = function (currentArticle, articleTypeKey) {
	const viewModel = this;
	const factory = viewModel.articleFactories[(currentArticle || {}).ArticleTypeKey] || function(x) { return window.database.CrmArticle_Article.CrmArticle_Article.create(x) };
	const article = factory(currentArticle);
	article.ArticleGroup01Key = window.Helper.Lookup.getDefaultLookupValueSingleSelect(viewModel.lookups.articleGroups01, article.ArticleGroup01Key);
	article.ArticleGroup02Key = window.Helper.Lookup.getDefaultLookupValueSingleSelect(viewModel.lookups.articleGroups02, article.ArticleGroup02Key);
	article.ArticleGroup03Key = window.Helper.Lookup.getDefaultLookupValueSingleSelect(viewModel.lookups.articleGroups03, article.ArticleGroup03Key);
	article.ArticleGroup04Key = window.Helper.Lookup.getDefaultLookupValueSingleSelect(viewModel.lookups.articleGroups04, article.ArticleGroup04Key);
	article.ArticleGroup05Key = window.Helper.Lookup.getDefaultLookupValueSingleSelect(viewModel.lookups.articleGroups05, article.ArticleGroup05Key);
	article.ArticleTypeKey = articleTypeKey;
	article.IsEnabled = true;
	article.CurrencyKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(viewModel.lookups.currencies, article.CurrencyKey);
	article.QuantityUnitKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(viewModel.lookups.quantityUnits, article.QuantityUnitKey);
	article.VATLevelKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(viewModel.lookups.vatLevels, article.VATLevelKey);
	window.database.add(article);
	viewModel.article(article.asKoObservable());
	const subscription = viewModel.article().ArticleTypeKey.subscribe(articleTypeKey => {
		subscription.dispose();
		const currentArticle = viewModel.article().getEntity();
		window.database.detach(currentArticle);
		viewModel.initNewArticle(currentArticle, articleTypeKey);
	});
};
namespace("Crm.Article.ViewModels").ArticleCreateViewModel.prototype.init = async function (id) {
	const viewModel = this;
	await window.Helper.Lookup.getLocalizedArrayMaps(viewModel.lookups);
	if (id) {
		const article = await window.database.CrmArticle_Article.find(id);
		window.database.attachOrGet(article);
		viewModel.article(article.asKoObservable());
	} else {
		let defaultArticleTypeKey = window.Helper.Lookup.getDefaultLookupValueSingleSelect(viewModel.lookups.articleTypes, "Material");
		viewModel.initNewArticle(null, defaultArticleTypeKey);
	}
	viewModel.article().ItemNo.extend({
		validation: {
			async: true,
			validator: function(itemNo, params, callback) {
				if (!itemNo) {
					callback(true);
					return;
				}
				window.database.CrmArticle_Article
					.filter(function (article) { return article.ItemNo === this.itemNo; }, { itemNo: itemNo })
					.toArray()
					.then(function(articles) {
						callback(articles.length === 0 || articles[0].Id === viewModel.article().Id());
					});
			},
			message: window.Helper.String.getTranslatedString("RuleViolation.Unique").replace("{0}", window.Helper.String.getTranslatedString("ItemNo"))
		}
	});
	await window.database.CrmArticle_ArticleDescription
		.filter(function (description) { return description.Key === this.Key; }, { Key: viewModel.article().ItemNo() })
		.toArray(function(articleDescriptions) {
			viewModel.articleDescriptions(articleDescriptions);
			viewModel.descriptions(viewModel.lookups.languages.$array.slice(1).map(function(x) {
				var articleDescription = articleDescriptions.find(function(it) {
					return it.Language === x.Key;
				});
				if (articleDescription) viewModel.translation(true);
				return {
					Value: window.ko.observable(articleDescription ? articleDescription.Value : ""),
					Language: x.Key,
					Name: x.Value,
					ArticleDescription: articleDescription
				};
			}));
		});
};
namespace("Crm.Article.ViewModels").ArticleCreateViewModel.prototype.toggleTranslations = function () {
	var viewModel = this;
	viewModel.translation(!viewModel.translation());
};
namespace("Crm.Article.ViewModels").ArticleCreateViewModel.prototype.cancel = function () {
	window.history.back();
};
namespace("Crm.Article.ViewModels").ArticleCreateViewModel.prototype.submit = function () {
	var viewModel = this;
	if (viewModel.article().ItemNo()) {
		viewModel.article().ItemNo(viewModel.article().ItemNo().trim());
		viewModel.article().Name(viewModel.article().ItemNo());
	}

	viewModel.loading(true);
	if (viewModel.errors().length > 0) {
		viewModel.loading(false);
		viewModel.errors.showAllMessages();
		viewModel.errors.scrollToError();
		return;
	}

	if (viewModel.article().ItemNo.isValidating()) {
		viewModel.article().ItemNo.isValidating.subscribe(function() {
			viewModel.submit.call(viewModel);
		});
		return;
	}

	viewModel.descriptions().forEach(function (it) {
		if (viewModel.articleDescriptions()
			.map(function (x) {
				return x.Language;
			})
			.includes(it.Language)) {
			const articleDescription = viewModel.articleDescriptions().find(function (x) {
				return x.Language === it.Language;
			});
			window.database.attachOrGet(articleDescription);
			articleDescription.Key = viewModel.article().ItemNo();
			articleDescription.Value = window.ko.unwrap(it.Value);
			if (!window.ko.unwrap(it.Value)) {
				window.database.remove(articleDescription);
			}
		} else if (window.ko.unwrap(it.Value)) {
			const articleDescription = window.database.CrmArticle_ArticleDescription.CrmArticle_ArticleDescription.create();
			articleDescription.Key = viewModel.article().ItemNo();
			articleDescription.Language = it.Language;
			articleDescription.Value = window.ko.unwrap(it.Value);
			window.database.add(articleDescription);
		}
	});
	window.database.saveChanges().then(function () {
		window.location.hash = "/Crm.Article/Article/DetailsTemplate/" + viewModel.article().Id();
	});
};
