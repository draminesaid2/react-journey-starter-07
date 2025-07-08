///<reference path="../../../../Content/@types/index.d.ts"/>

import { ArticleDetailsViewModel } from "../ts/ArticleDetailsViewModel";
import { ArticleDetailsRelationshipsTabViewModel } from "../ts/ArticleDetailsRelationshipsTabViewModel";
import { ArticleDetailsDocumentsTabViewModel } from "../ts/ArticleDetailsDocumentsTabViewModel";
import { ArticleRelationshipEditModalViewModel } from "../ts/ArticleRelationshipEditModalViewModel";
import { ProductFamilyCreateViewModel } from "../ts/ProductFamilyCreateViewModel";
import { ProductFamilyDetailsViewModel } from "../ts/ProductFamilyDetailsViewModel";

declare global {
	namespace Crm {
		namespace Article {
			namespace ViewModels {
				let ArticleDetailsViewModel: ArticleDetailsViewModel;
				let ArticleDetailsRelationshipsTabViewModel: ArticleDetailsRelationshipsTabViewModel;
				let ArticleDetailsDocumentsTabViewModel: ArticleDetailsDocumentsTabViewModel;
				let ArticleRelationshipEditModalViewModel: ArticleRelationshipEditModalViewModel;
				let ProductFamilyCreateViewModel: ProductFamilyCreateViewModel;
				let ProductFamilyDetailsViewModel: ProductFamilyDetailsViewModel;
			}
		}
	}
}
