(function($data) {
	$data.Queryable.prototype.specialFunctions.filterByDynamicFormTitle = {
		"oData": function(urlSearchParams, data) {
			var filter = (data.filter || "").trim();
			var statusKey = (data.statusKey || "").trim();
			urlSearchParams.append("filterByDynamicFormTitle", filter);
			urlSearchParams.append("filterByDynamicFormStatusKey", statusKey );
		},
		"webSql": function(query, data) {
			var filter = (data.filter || "").trim();
			var statusKey = (data.statusKey || "").trim();
			var it = "it";
			if (query.defaultType !== window.database.CrmDynamicForms_DynamicForm.elementType) {
				it = "it.DynamicForm";
			}
			var regex = new RegExp(/\$it/, 'g');
			query = query
				.filter("this.statusKey ==  null || this.statusKey === '' || $it.Languages.StatusKey === this.statusKey".replace(regex, it), { statusKey: statusKey })
				.filter("$it.Localizations.DynamicFormElementId == null".replace(regex, it))
				.filter("$it.Localizations.Language === $it.DefaultLanguageKey || $it.Localizations.Language === this.languageKey".replace(regex, it), { languageKey: data.languageKey })
				.filter("this.filter ==  null || this.filter === '' || $it.Localizations.Value.toLowerCase().contains(this.filter)".replace(regex, it), { filter: filter });
			return query;
		}
	};
	$data.Queryable.prototype.includeDynamicFormElements = function(){
		if (this.entityContext.storageProvider.name === "oData"){
			return this.include("DynamicForm.Elements");
		}
		return this;
	}
	$data.Queryable.prototype.includeElements = function(){
		if (this.entityContext.storageProvider.name === "oData"){
			return this.include("Elements");
		}
		return this;
	}
	$data.Queryable.prototype.specialFunctions.orderByDynamicFormTitle = {
		"oData": function(urlSearchParams, data) {
			urlSearchParams.append("orderByDynamicFormTitleLanguage", data.language);
			urlSearchParams.append("orderByDynamicFormTitleDirection", data.direction);
		}
	};
})($data);