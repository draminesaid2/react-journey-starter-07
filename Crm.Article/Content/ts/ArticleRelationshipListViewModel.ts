///<reference path="../../../../Content/@types/index.d.ts" />
import { namespace } from "../../../../Content/ts/namespace";

export class ArticleRelationshipListViewModel extends window.Main.ViewModels.GenericListViewModel {

	args: any;

	constructor(args) {
		super("CrmArticle_ArticleRelationship", ["RelationshipTypeKey", "CreateDate"], ["DESC", "DESC"], ["Parent", "Parent.DocumentAttributes.FileResource", "Parent.Tags", "Child", "Child.Tags", "Child.DocumentAttributes.FileResource"]);
		this.args = args;
	}

	deleteRelationship = window.Main.ViewModels.BaseRelationshipsTabViewModel.prototype.deleteRelationship;

	async init(): Promise<void> {
		await super.init();
	}

	applyFilters(query) {
		return super.applyFilters(query).filter("it.ParentId === this.id || it.ChildId === this.id", { id: this.args.article().Id() });
	}

	getItemGroup(item) {
		if (item.ParentId() !== this.args.article().Id() && item.ChildId() === this.args.article().Id()) {
			if (item.RelationshipTypeKey() === "VariableValue") {
				return { title: window.Helper.String.getTranslatedString("VariableValue") };
			} else if (item.RelationshipTypeKey() === "Accessory") {
				return { title: window.Helper.String.getTranslatedString("AccessoryOf") };
			}
			return { title: "" };
		} else {
			const value = this.args.lookups.articleRelationshipTypes[`${item.RelationshipTypeKey()}`].Value;
			return { title: value };
		}
	}

	getInverseRelationship(relationship) {
		return $.Deferred().resolve().promise();
	}

}

namespace("Crm.Article.ViewModels").ArticleRelationshipListViewModel = ArticleRelationshipListViewModel;
