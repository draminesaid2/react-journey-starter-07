///<reference path="../../../../Content/@types/index.d.ts" />
import { HelperLookup } from "../../../../Content/ts/helper/Helper.Lookup";
import { namespace } from "../../../../Content/ts/namespace";

export class ArticleRelationshipEditModalViewModel extends window.Main.ViewModels.BaseRelationshipEditModalViewModel {

	lookups = {
		articleRelationshipTypes: { $tableName: "CrmArticle_ArticleRelationshipType" }
	};
	table: any = window.database.CrmArticle_ArticleRelationship;
	relationshipTypeLookup = this.lookups.articleRelationshipTypes;
	articleType: string;
	constructor(args) {
		super(args);
	}

	async init(id, params){
		super.init(id);
		this.currentUser = ko.observable(await window.Helper.User.getCurrentUser());
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		if (params.articleType) {
			this.articleType = params.articleType;
		}
	};

	getAutoCompleterCaption() {
		return "Article";
	};

	getAutoCompleterOptions() {
		return {
			table: "CrmArticle_Article",
			mapDisplayObject: window.Helper.Article.mapArticleForSelect2Display,
			key: "Id"
		};
	};

	getAutocompleteOptionsRelationshipType() {
		const self = this;
		return {
			customFilter: function (query) {
				if (self.articleType !== "Variable") {
					return query.filter("it.Key !== this.key && it.Language === this.language",
						{ key: "VariableValue", language: self.currentUser().DefaultLanguageKey });
				}
				return query.filter("it.Language === this.language", { language: self.currentUser().DefaultLanguageKey });
			},
			table: "CrmArticle_ArticleRelationshipType",
			mapDisplayObject: HelperLookup.mapLookupForSelect2Display,
			getElementByIdQuery: HelperLookup.getLookupByKeyQuery
		};
	}

	getQueryForEditing() {
		return window.database.CrmArticle_ArticleRelationship.include("Child");
	};

	getEditableId(relationship) {
		return relationship.ChildId;
	};

	async save() {
		const key = this.relationship().RelationshipTypeKey();
		const hasQuantity = this.lookups.articleRelationshipTypes[key].HasQuantity;

		try {
			this.loading(true)
			if (hasQuantity) {
				const article = await window.database.CrmArticle_Article.find(this.relationship().ChildId());
				const quanityUnit = article.QuantityUnitKey == null ? "" : article.QuantityUnitKey;
				this.relationship().QuantityValue(1);
				this.relationship().QuantityUnitKey(quanityUnit);
			} else {
				this.relationship().QuantityValue(0);
			}

			await super.save();
			this.loading(false);

		} catch (e) {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("Error"), (e as Error).message, "error");
		}

	}
}

namespace("Crm.Article.ViewModels").ArticleRelationshipEditModalViewModel = ArticleRelationshipEditModalViewModel;
