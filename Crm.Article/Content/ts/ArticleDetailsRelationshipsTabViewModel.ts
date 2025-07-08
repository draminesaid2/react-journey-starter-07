///<reference path="../../../../Content/@types/index.d.ts" />
import type {GenericListViewModel} from "../../../../Content/@types";
import {namespace} from "../../../../Content/ts/namespace";

export class ArticleDetailsRelationshipsTabViewModel extends window.Main.ViewModels.BaseRelationshipsTabViewModel {

	genericArticleRelationships: GenericListViewModel;
	articleRelationships: KnockoutObservableArray<any> = ko.observableArray([]);
	genericArticleCompanyRelationships: GenericListViewModel;
	articleCompanyRelationships: KnockoutObservableArray<any> = ko.observableArray([]);
	articleId: string;
	lookups = {
		regions: {$tableName: "Main_Region"},
		companyTypes: {$tableName: "Main_CompanyType"},
		countries: {$tableName: "Main_Country"},
		articleCompanyRelationshipTypes: {$tableName: "CrmArticle_ArticleCompanyRelationshipType"}
	};

	constructor(parentViewModel: any) {
		super(parentViewModel);
		this.articleId = parentViewModel.article().Id();

		this.genericArticleRelationships = new window.Crm.Article.ViewModels.ArticleRelationshipListViewModel(parentViewModel);
		this.articleRelationships = this.genericArticleRelationships.items;
		this.subViewModels.push(this.genericArticleRelationships);

		this.genericArticleCompanyRelationships = new window.Crm.Article.ViewModels.ArticleCompanyRelationshipListViewModel(this.articleId);
		this.articleCompanyRelationships = this.genericArticleCompanyRelationships.items;
		// @ts-ignore
		this.articleCompanyRelationships.distinct("RelationshipTypeKey");
		this.subViewModels.push(this.genericArticleCompanyRelationships);

	}

	async init(): Promise<void> {
		this.loading(true);
		await super.init();
		return window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
	}

	getInverseRelationship(relationship) {
		return $.Deferred().resolve().promise();
	}

}

namespace("Crm.Article.ViewModels").ArticleDetailsRelationshipsTabViewModel = ArticleDetailsRelationshipsTabViewModel;
