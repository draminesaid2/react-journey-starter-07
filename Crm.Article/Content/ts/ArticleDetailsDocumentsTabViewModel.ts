///<reference path="../../../../Content/@types/index.d.ts" />
import { namespace } from "../../../../Content/ts/namespace";

export class ArticleDetailsDocumentsTabViewModel extends window.Main.ViewModels.ContactDetailsDocumentsTabViewModel {

	constructor(args) {
		super(window.Crm.Article.ViewModels.ArticleDetailsDocumentsTabViewModel);
		this.contactId(args.contactId());
	}
}

namespace("Crm.Article.ViewModels").ArticleDetailsDocumentsTabViewModel = ArticleDetailsDocumentsTabViewModel;
