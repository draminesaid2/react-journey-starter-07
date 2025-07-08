(function($data) {
	$data.Queryable.prototype.specialFunctions.filterByAttributeForm= {
			"oData": function (urlSearchParams, params) {
				var filters = params.filters || [];
				filters = filters.filter(function (x) { return x.Response() !== null && x.Response().toString().length > 0; });
				if (Array.isArray(filters)) {
					filters.forEach(function (filterElement) {
						var value = filterElement.Response();
						if (filterElement.FormElementType() === "Date")
							value = value.toISOString();
						if (filterElement.FormElementType() === "Time"){
							value = moment.duration(value).format('hh:mm');
						}
						urlSearchParams.append("filterByAttributeFormProperty_" + filterElement.Id(), value);
					});
				}
			},
			"webSql": function (query, params) {
				var filters = params.filters || [];
				filters = filters.filter(function (x) { return x.Response() !== null && x.Response().toString().length > 0; });
				if (Array.isArray(filters)) {
					filters.forEach(function (filterElement) {
						var filterContactKeys = window.database.CrmAttributeForms_AttributeForm
							.include("DynamicForm.Languages")
							.include("Responses")
							.filter("it.Responses.DynamicFormKey === this.dynamicFormKey", { dynamicFormKey: filterElement.DynamicFormKey()})
							.filter("!it.Completed")
							.filter("it.Responses.DynamicFormElementKey === this.dynamicFormElementKey", { dynamicFormElementKey: filterElement.Id() });
						
						var filterValue = {
							Value: filterElement.Response(),
							Operator: '===',
							Column: 'Responses.Value'
						};


						if (filterElement.FormElementType() === "Time"){
							filterValue.Value = moment.duration(filterElement.Response()).format('hh:mm');
						}
						if (filterElement.FormElementType() === "SingleLineText" || filterElement.FormElementType() === "MultiLineText" || filterElement.FormElementType() === "Date")
							filterValue = filterElement.Response();

						filterContactKeys = window.Main.ViewModels.GenericListViewModel.prototype.applyFilter(filterContactKeys, filterValue, "Responses.Value");
						filterContactKeys = filterContactKeys.map(function (x) { return x.ReferenceKey; });
						query = query.filter(function (x) { return x.Id in this.ids; }, { ids: filterContactKeys });
					});
				}
				return query;
			}
	};
})($data);