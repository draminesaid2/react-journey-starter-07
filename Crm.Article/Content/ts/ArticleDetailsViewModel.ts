/* eslint-disable camelcase */
///<reference path="../../../../Content/@types/index.d.ts" />
import { Breadcrumb } from "../../../../Content/ts/breadcrumbs";
import { namespace } from "../../../../Content/ts/namespace";

export class ArticleDetailsViewModel extends window.Main.ViewModels.ContactDetailsViewModel {

	descriptions = ko.observableArray<any>([]);
	currentUser = ko.observable<Crm.Rest.Model.Main_User>(null);
	article = ko.observable<Crm.Article.Rest.Model.ObservableCrmArticle_Article>(null);

	lookups = {
		languages: { $tableName: "Main_Language", $filterExpression: "it.IsSystemLanguage === true", $array: undefined },
		articleGroups01: { $tableName: "CrmArticle_ArticleGroup01" },
		articleGroups02: { $tableName: "CrmArticle_ArticleGroup02" },
		articleGroups03: { $tableName: "CrmArticle_ArticleGroup03" },
		articleGroups04: { $tableName: "CrmArticle_ArticleGroup04" },
		articleGroups05: { $tableName: "CrmArticle_ArticleGroup05" },
		articleTypes: { $tableName: "CrmArticle_ArticleType" },
		currencies: { $tableName: "Main_Currency" },
		articleRelationshipTypes: { $tableName: "CrmArticle_ArticleRelationshipType" },
		quantityUnit: { $tableName: "CrmArticle_QuantityUnit" },
		vatLevel: { $tableName: "CrmArticle_VATLevel" },
		regions: { $tableName: "Main_Region" },
		companyTypes: { $tableName: "Main_CompanyType" },
		countries: { $tableName: "Main_Country" },
		articleCompanyRelationshipTypes: { $tableName: "CrmArticle_ArticleCompanyRelationshipType" }
	};

	constructor() {
		super();
		this.contactType("Article");
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		this.currentUser(await window.Helper.User.getCurrentUserPromise());
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);

		const articleData = await window.database.CrmArticle_Article
			.include("ProductFamily")
			.include2("Tags.orderBy(function(x) { return x.Name; })")
			.find(id);

		this.article(articleData.asKoObservable());
		
		this.contactId(this.article().Id());

		await this.setBreadcrumbs(id);
		this.contact(this.article());

		const descriptions = await this.getArticleDescriptions(this.article().ItemNo());
		const articleDescriptionObjects = await this.createDescriptionObjects(descriptions);
		this.descriptions(articleDescriptionObjects);
	}

	async getArticleDescriptions(key: string): Promise<Crm.Article.Model.Lookups.CrmArticle_ArticleDescription[]> {
		return await window.database.CrmArticle_ArticleDescription
			.filter("it.Key == this.Key", { Key: key })
			.toArray();
	}

	createDescriptionObjects(descriptions: Crm.Article.Model.Lookups.CrmArticle_ArticleDescription[]): Promise<any> {
		return this.lookups.languages.$array.slice(1).map(function (x) {
			const articleDescription = descriptions.find(function (it) {
				return it.Language === x.Key;
			});
			return {
				Value: window.ko.observable(articleDescription ? articleDescription.Value : ""),
				Language: x.Key,
				Name: x.Value,
				ArticleDescription: articleDescription
			};
		});

	}

	async setBreadcrumbs(id: string): Promise<void> {
		await window.breadcrumbsViewModel.setBreadcrumbs([
			new Breadcrumb(window.Helper.String.getTranslatedString("Article"), "#/Crm.Article/ArticleList/IndexTemplate"),
			new Breadcrumb(window.Helper.Article.getArticleAutocompleteDisplay(this.article()), window.location.hash, null, id)
		]);
	}

	onSave(): void {
		const viewModel = this;
		viewModel.descriptions().forEach(function (description) {
			if (window.ko.unwrap(description.Value) === "" && description.ArticleDescription === undefined)
				return;
			if (description.ArticleDescription) {
				window.database.attachOrGet(description.ArticleDescription);
				description.ArticleDescription.Value = window.ko.unwrap(description.Value);
				description.ArticleDescription.Key = viewModel.article().ItemNo();
				description.ArticleDescription.Language = description.Language;
				if (!window.ko.unwrap(description.Value)) {
					window.database.remove(description.ArticleDescription);
				}
			} else {
				const articleDescription = window.database.CrmArticle_ArticleDescription.defaultType.create();
				articleDescription.Key = viewModel.article().ItemNo();
				articleDescription.Language = description.Language;
				articleDescription.Value = window.ko.unwrap(description.Value);
				window.database.add(articleDescription);
			}
		});
	}

	async reloadProductFamily(): Promise<void> {
		const id = this.article().ProductFamilyKey();
		if (id) {
			const productFamily = await window.database.CrmArticle_ProductFamily.find(id);
			this.article().ProductFamily(productFamily.asKoObservable());
		} else {
			this.article().ProductFamily(null);
		}
	}
}

namespace("Crm.Article.ViewModels").ArticleDetailsViewModel = ArticleDetailsViewModel;
