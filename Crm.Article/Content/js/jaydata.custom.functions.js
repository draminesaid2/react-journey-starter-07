(function($data) {
	$data.Queryable.prototype.specialFunctions.filterByArticleDescription = {
		"oData": function(urlSearchParams, data) {
			if (data.language && data.filter) {
				urlSearchParams.append("filterByArticleDescriptionLanguage", data.language);
				urlSearchParams.append("filterByArticleDescriptionFilter", data.filter);
			}
		},
		"webSql": function(query, data) {
			var queryFilter = [];
			var args = {};
			if (window.database.CrmArticle_ArticleDescription) {
				var articleDescriptionItemNos = window.database.CrmArticle_ArticleDescription
					.filter(function(it) { return it.Language === this.language && it.Value.contains(this.filter); }, { filter: data.filter, language: data.language })
					.map(function(it) { return it.Key; });
				queryFilter.push("it.ItemNo in this.articleDescriptionItemNos");
				args.articleDescriptionItemNos = articleDescriptionItemNos;
			}
			if (query.defaultType === window.database.CrmArticle_Article.elementType) {
				queryFilter.push("it.ItemNo.contains(this.filter)");
				queryFilter.push("it.Description.contains(this.filter)");
				args.filter = data.filter;
			} else if (window.database.CrmArticle_Article) {
				var articleItemNos = window.database.CrmArticle_Article
					.filter("it.Description.contains(this.filter)", { filter: data.filter })
					.map("it.ItemNo");
				queryFilter.push("it.ItemNo in this.articleItemNos");
				args.articleItemNos = articleItemNos;
			}
			return query.filter(queryFilter.join(" || "), args);
		}
	};

	$data.Queryable.prototype.specialFunctions.filterByProductFamilyDescription = {
		"oData": function (urlSearchParams, data) {
			if (data.language && data.filter) {
				urlSearchParams.append("filterByProductFamilyDescriptionLanguage", data.language);
				urlSearchParams.append("filterByProductFamilyDescriptionFilter", data.filter);
			}
		},
		"webSql": function (query, data) {
			var queryFilter = [];
			var args = {};
			if (window.database.CrmArticle_ProductFamilyDescription) {
				var productfamilyDescriptionNames = window.database.CrmArticle_ProductFamilyDescription
					.filter(function (it) { return it.Language === this.language && it.Value.contains(this.filter); }, { filter: data.filter, language: data.language })
					.map(function (it) { return it.Key; });
				queryFilter.push("it.Name in this.productfamilyDescriptionNames");
				args.productfamilyDescriptionNames = productfamilyDescriptionNames;
			}
			if (query.defaultType === window.database.CrmArticle_ProductFamily.elementType) {
				queryFilter.push("it.Name.contains(this.filter)");
				args.filter = data.filter;
			} else if (window.database.CrmArticle_Article) {
				var productfamilyNames = window.database.CrmArticle_Article
					.filter("it.Name.contains(this.filter)", { filter: data.filter })
					.map("it.Name");
				queryFilter.push("it.Name in this.productfamilyNames");
				args.productfamilyNames = productfamilyNames;
			}
			return query.filter(queryFilter.join(" || "), args);
		}
	};
})($data);