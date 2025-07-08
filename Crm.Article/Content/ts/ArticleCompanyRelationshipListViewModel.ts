///<reference path="../../../../Content/@types/index.d.ts" />
import { namespace } from "../../../../Content/ts/namespace";

export class ArticleCompanyRelationshipListViewModel extends window.Main.ViewModels.GenericListViewModel {


	constructor(articleId) {
		super("CrmArticle_ArticleCompanyRelationship"
			, ["RelationshipTypeKey", "Child.Name"], ["ASC", "ASC"]
			, ["Child", {
				Selector: "Child.Addresses",
				Operation: "filter(function (a) {return a.IsCompanyStandardAddress == true;})"
			}, "Child.ResponsibleUserUser", "Child.Tags", "Child.ParentCompany"]);
		this.getFilter("ParentId").extend({ filterOperator: "===" })(articleId);
	}

	async init(): Promise<void> {
		await super.init();
	}

}

namespace("Crm.Article.ViewModels").ArticleCompanyRelationshipListViewModel = ArticleCompanyRelationshipListViewModel;
