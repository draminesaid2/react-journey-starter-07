class HelperProductFamily {

	static filterParent(query, term, id) {
		if (term) {
			query = query.filter(function (it) {
					return it.Name.toLowerCase().contains(this.term);
				},
				{ term: term });
		}
		query = query.filter("it.Id !== this.id && it.ParentId !== this.id", { id: id});
		return query
			.orderByDescending("it.Name");
	}

	static filterChild(query, term, id) {
		if (term) {
			query = query.filter(function (it) {
					return it.Name.toLowerCase().contains(this.term);
				},
				{ term: term });
		}
		query = query.filter("it.Id !== this.id && it.ParentId == null", { id: id });
		return query
			.orderByDescending("it.Name");
	}

	static getProductFamilyAutocompleteFilter(query, filter, language) {
		if (filter) {
			return query.filter("filterByProductFamilyDescription", { language: language, filter: filter });
		}
		return query;
	}

	static loadProductFamilyDescriptions(results, language) {
		if (!window.database.CrmArticle_ProductFamilyDescription) {
			return results;
		}
		const productfamilies = results.reduce(function (map, productfamily) {
			productfamily = Helper.Database.getDatabaseEntity(productfamily);
			map[productfamily.Id] = productfamily;
			return map;
		}, {});
		return window.database.CrmArticle_ProductFamilyDescription
			.filter("it.Key in this.Ids && it.Language === this.language", { Ids: Object.keys(productfamilies), language: language })
			.map(function (x) { return { Key: x.Key, Value: x.Value }; })
			.forEach(function (description) {
				const productfamily = productfamilies[description.Key];
				if (productfamily && description.Value) {
					productfamilies[description.Key].$ProductFamilyDescription = description.Value;
				}
			})
			.then(function () {
				return results;
			});
	}

	static getProductFamilyAutocompleteDisplay(productfamily) {
		const productfamilyDescription = Helper.ProductFamily.getProductFamilyDescription(productfamily);
		if (productfamilyDescription) {
			return productfamilyDescription;
		}
		return ko.unwrap(productfamily.Name);
	}

	static getProductFamilyDescription(productfamily) {
		productfamily = Helper.Database.getDatabaseEntity(productfamily);
		return ko.unwrap(productfamily.$ProductFamilyDescription) || ko.unwrap(productfamily.Description);
	}

	static getProductFamilyStatusAbbrevation(productFamily) {
		return productFamily.substring(0, 1);
	}

	static getParent(productFamilyId, viewModel) {
		return window.database.CrmArticle_ProductFamily
			.find(productFamilyId)
			.then(function (result) {
				if (result.ParentId !== null) {
					return window.Helper.ProductFamily.getParent(result.ParentId, viewModel);
				}
				viewModel.parentProductFamily(result.asKoObservable());
				return result;
			});
		}
	}

(window.Helper = window.Helper || {}).ProductFamily = HelperProductFamily;
