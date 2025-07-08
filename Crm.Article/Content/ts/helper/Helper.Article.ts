///<reference path="../../../../../Content/@types/index.d.ts" />

import {HelperString} from "../../../../../Content/ts/helper/Helper.String";

export class HelperArticle {
	static getArticleTypeAbbreviation(article, articleTypes): string {
		const articleTypeKey = window.ko.unwrap(article.ArticleTypeKey);
		if (articleTypeKey) {
			const articleType = (articleTypes || {})[articleTypeKey];
			if (articleType && articleType.Value) {
				return articleType.Value[0];
			}
		}
		return "";
	}
	static getDiscountText(discountKey) {
		// @ts-ignore
		const attributes = Object.keys(window.Crm.Article.Model.Enums.DiscountType);
		// @ts-ignore
		const discountType = window.Crm.Article.Model.Enums.DiscountType;
		return attributes.filter(function (x) { return discountType[x] === discountKey;})[0];
	}
	static getArticleDescription(article) {
		const articleFromDb = window.Helper.Database.getDatabaseEntity(article);
		if (articleFromDb) {
			return ko.unwrap(article.$ArticleDescription) || ko.unwrap(article.Description);
		}
		return "";
	}

	/** @todo Potential dead code **/
	static getArticleDescriptionAbbreviation(article): string {
		const articleDescription = window.Helper.Article.getArticleDescription(article);
		if (articleDescription) {
			return articleDescription[0];
		}
		return "";
	}

	static getArticleAutocompleteDisplay(article): string {
		const articleDescription = window.Helper.Article.getArticleDescription(article);
		const displayName = [];
		if (window.ko.unwrap(article.IsEnabled) === false) {
			displayName.push("[" + HelperString.getTranslatedString("Inactive") + "]");
		}
		const ItemNo = HelperString.trim(ko.unwrap(article.ItemNo));
		displayName.push(ItemNo);
		if(articleDescription){
			displayName.push("-");
			displayName.push(HelperString.trim(articleDescription));
		}
		return displayName.join(" ");
	}
	
	static getArticleAutocompleteFilter(query, filter, language) {
		if (filter) {
			return query.filter("filterByArticleDescription", { language: language, filter: filter });
		}
		return query;
	}

	static loadArticleDescriptionsMapFromItemNo(items, language: string) {
		const itemNos = items.map(function (i) { return ko.unwrap(i.ItemNo); });
		return window.Helper.Article.loadArticleDescriptionsMap(itemNos, language).then(function (descriptions) {
			items.forEach(function (i) {
				const itemNo = ko.unwrap(i.ItemNo);
				let description = descriptions[itemNo] || "";
				if (!description && i.Description) {
					description = ko.unwrap(i.Description);
				}
				if (ko.isObservable(i.ItemDescription)) {
					i.ItemDescription(description);
				} else {
					i.ItemDescription = ko.observable(description);
				}
			});
			return items;
		});
	}
	static loadArticleDescriptionsMap(itemNos, language: string) {
		const descriptions = itemNos.reduce(function (map, i) { map[i] = null; return map; }, {});
		const queries = [];
		if (window.database.CrmArticle_ArticleDescription) {
			queries.push({
				queryable: window.database.CrmArticle_ArticleDescription
					.filter("it.Key in this.itemNos && it.Language === this.language", { itemNos: itemNos, language: language })
					.map(function (x) { return { Key: x.Key, Value: x.Value }; }),
				method: "toArray",
				handler: function (items) {
					items.forEach(function (i) {
						descriptions[i.Key] = i.Value;
					});
				}
			});
		}
		if (window.database.CrmArticle_Article) {
			queries.push({
				queryable: window.database.CrmArticle_Article
					.filter("it.ItemNo in this.itemNos", { itemNos: itemNos })
					.map(function (x) { return { ItemNo: x.ItemNo, Description: x.Description }; }),
				method: "toArray",
				handler: function (items) {
					items.forEach(function (i) {
						if (!descriptions[i.ItemNo]) {
							descriptions[i.ItemNo] = i.Description;
						}
					});
				}
			});
		}
		return window.Helper.Batch.Execute(queries).then(function () { return descriptions; });
	}

	static loadArticleDescriptions(results, language: string) {
		if (!window.database.CrmArticle_ArticleDescription) {
			return results;
		}
		const articles = results.reduce(function (map, article) {
			article = window.Helper.Database.getDatabaseEntity(article);
			map[article.ItemNo] = article;
			return map;
		}, {});
		return window.database.CrmArticle_ArticleDescription
			.filter("it.Key in this.itemNos && it.Language === this.language", { itemNos: Object.keys(articles), language: language })
			.map(function (x) { return { Key: x.Key, Value: x.Value }; })
			.forEach(function (description) {
				const article = articles[description.Key];
				if (article && description.Value) {
					articles[description.Key].$ArticleDescription = description.Value;
				}
			})
			.then(function () {
				return results;
			});
	}
	static mapArticleForSelect2Display(article) {
		return {
			id: article.Id,
			item: article.asKoObservable(),
			text: window.Helper.Article.getArticleAutocompleteDisplay(article)
		};
	}
	static getAutocompleteOptionsShorthand(viewModel, withoutKey: boolean, showActiveArticles, showExpiredArticles, showUpcomingArticles) {
		const language = (document.getElementById("meta.CurrentLanguage") as any).content;
		
		let options = window.Helper.Article.getArticleSelect2Options(viewModel, withoutKey);
		options.joins = [
			{
				Selector: "DocumentAttributes",
				Operation: "filter(function(x){ return x.UseForThumbnail === true; })"
			}, "DocumentAttributes.FileResource"];
		const baseFilter = options.customFilter;
		options.customFilter = function (query, filter) {
			let showActive = window.ko.unwrap(showActiveArticles);
			let showExpired = window.ko.unwrap(showExpiredArticles);
			let showUpcoming = window.ko.unwrap(showUpcomingArticles);
			query = baseFilter(query, filter, language)

			if(!showActive && !showExpired && !showUpcoming){
				return query.filter("false");
			}
			
			let today = new Date().setHours(0,0,0,0)
			const conditions = {
				activeCondition: "(it.IsEnabled === true)",
				expiredCondition: "(it.ValidTo < this.today || (it.IsEnabled === false && it.ValidTo == null && it.ValidFrom == null))",
				upcomingCondition: "(it.ValidFrom >= this.today)",
			}
			let activeConditions = [];
			if (showActive){
				activeConditions.push(conditions.activeCondition)
			}
			if (showExpired){
				activeConditions.push(conditions.expiredCondition)
			}
			if (showUpcoming){
				activeConditions.push(conditions.upcomingCondition)
			}
			return query.filter(activeConditions.join(" || "), {today: today});
		}
		return options;
	}

	static getArticleSelect2Options(viewModel, withoutKey: boolean) {
		const options: any = {
			table: "CrmArticle_Article",
			orderBy: ["ItemNo"],
			mapDisplayObject: function (article) {
				if (viewModel.getArticleAutocompleteDisplay) {
					return {
						id: article.Id,
						text: viewModel.getArticleAutocompleteDisplay(article)
					};
				} else {
					return {
						id: article.Id,
						text: window.Helper.Article.getArticleAutocompleteDisplay(article),
						item: article.asKoObservable()
					};
				}
			}
		};
		if (withoutKey !== true) {
			options.key = "Id";
		}
		if (viewModel) {
			const language = (document.getElementById("meta.CurrentLanguage") as any).content;
			if (viewModel.getArticleSelect2Filter) {
				options.customFilter = viewModel.getArticleSelect2Filter.bind(viewModel);
			} else {
				options.customFilter = function (query, filter) { return window.Helper.Article.getArticleAutocompleteFilter(query, filter, language); };
			}
			if (viewModel.onArticleSelect) {
				options.onSelect = viewModel.onArticleSelect.bind(viewModel);
			}
			if (viewModel.onArticleAutocompleteResult) {
				options.onResult = viewModel.onArticleAutocompleteResult.bind(viewModel);
			} else {
				options.onResult = function (results) { return window.Helper.Article.loadArticleDescriptions(results, language); };
			}
		}
		return options;
	}
}
